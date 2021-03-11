using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// 描 述：车辆进出记录表
    /// </summary>
    public class CarinlogService : RepositoryFactory<CarinlogEntity>, CarinlogIService
    {
        #region 获取数据

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //根据车辆筛选
            if (!queryParam["CID"].IsEmpty())
            {
                string CID = queryParam["CID"].ToString();

                pagination.conditionJson += string.Format(" and CID = '{0}'", CID);

            }

            //根据时间进行筛选
            if (!queryParam["Time"].IsEmpty())
            {
                string Time = queryParam["Time"].ToString();

                pagination.conditionJson += string.Format(" and TO_CHAR(CREATEDATE,'yyyy-MM-dd') = '{0}'", Time);

            }

            //根据时间进行筛选
            if (!queryParam["startTime"].IsEmpty() && !queryParam["endTime"].IsEmpty())
            {
                string startTime = queryParam["startTime"].ToString() + " 00:00:00";
                string endTime = queryParam["endTime"].ToString() + " 23:59:59";

                pagination.conditionJson +=
                    string.Format(
                        " and TO_CHAR(CREATEDATE,'yyyy-MM-dd HH24:mm:ss') >= '{0}' and  TO_CHAR(CREATEDATE,'yyyy-MM-dd HH24:mm:ss') <= '{1}' ",
                        startTime, endTime);

            }

            //根据进出场进行筛选
            if (!queryParam["Status"].IsEmpty())
            {
                string Status = queryParam["Status"].ToString();

                pagination.conditionJson += string.Format(" and STATUS = {0}", Status);
            }
            //车辆类型
            if (!queryParam["CarType"].IsEmpty())
            {
                string CarType = queryParam["CarType"].ToString();

                pagination.conditionJson += string.Format(" and log.TYPE = {0}", CarType);
            }
            //通行门岗
            if (!queryParam["CurrentName"].IsEmpty())
            {
                string name = queryParam["CurrentName"].ToString();
                pagination.conditionJson += string.Format(" and log.ADDRESS like '%{0}%'", name);
            }

            if (!queryParam["condition"].IsEmpty())
            {
                //车牌号
                if (queryParam["condition"].ToString() == "CarNo")
                {
                    string CarNo = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and log.CARNO like '%{0}%'", CarNo);
                }
                //驾驶人
                if (queryParam["condition"].ToString() == "Dirver")
                {
                    string Dirver = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and log.DRIVERNAME like '%{0}%'", Dirver);
                }
                //电话号码
                if (queryParam["condition"].ToString() == "Phone")
                {
                    string Phone = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and log.PHONE  like '{0}%'", Phone);
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 根据车牌号获取最新进场信息
        /// </summary>
        /// <param name="CarNo"></param>
        /// <returns></returns>
        public CarinlogEntity GetNewCarinLog(string CarNo)
        {
            return BaseRepository().IQueryable(it => it.CarNo == CarNo && it.IsLeave == 0 && it.Status == 0).OrderByDescending(it => it.CreateDate).FirstOrDefault();
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataTable GetTableChar(string year = "")
        {
            string sql = string.Format("select TYPE, count(TYPE) carnum,0.00 as bl from bis_carinlog where STATUS=0 ");
            if (!string.IsNullOrEmpty(year))
            {
                string[] arrDate = year.Split('|');
                sql += string.Format(" and (createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(arrDate[0]).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(arrDate[1]).ToString("yyyy-MM-dd 23:59:59"));
            }
            sql += " group by TYPE  order by TYPE asc";
            DataTable dt = BaseRepository().FindTable(sql);

            string countsql = string.Format("select count(TYPE) carnum from bis_carinlog where STATUS=0 ");
            if (!string.IsNullOrEmpty(year))
            {
                string[] arrDate = year.Split('|');
                countsql += string.Format(" and (createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(arrDate[0]).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(arrDate[1]).ToString("yyyy-MM-dd 23:59:59"));
            }

            int count = Convert.ToInt32(BaseRepository().FindObject(countsql));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][2] = Convert.ToInt32(dt.Rows[i][1]) / Convert.ToDouble(count) * 100;
            }

            return dt;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CarinlogEntity> GetList(string Cid)
        {
            return this.BaseRepository().IQueryable(it => it.CID == Cid).OrderBy(it => it.CreateDate).ToList();
        }


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CarinlogEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取车辆出入统计图
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        public string GetLogChart(string year = "")
        {
            List<object[]> list = new List<object[]>();
            string sql = string.Format("select TYPE, count(TYPE) carnum from bis_carinlog where STATUS=0 ");
            if (!string.IsNullOrEmpty(year))
            {
                string[] arrDate = year.Split('|');
                sql += string.Format(" and (createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(arrDate[0]).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(arrDate[1]).ToString("yyyy-MM-dd 23:59:59"));
            }
            sql += " group by TYPE order by TYPE asc";
            DataTable dt = this.BaseRepository().FindTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int type = Convert.ToInt32(dt.Rows[i][0]);
                string carname = "";
                switch (type)
                {
                    case 0:
                        carname = "电厂班车";
                        break;
                    case 1:
                        carname = "私家车";
                        break;
                    case 2:
                        carname = "商务公车";
                        break;
                    case 3:
                        carname = "拜访车辆";
                        break;
                    case 4:
                        carname = "物料车辆";
                        break;
                    case 5:
                        carname = "危化品车辆";
                        break;
                }
                object[] arr = { carname, Convert.ToInt32(dt.Rows[i][1]) };
                list.Add(arr);

            }
            dt.Dispose();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// 获取车辆出入场信息
        /// </summary>
        /// <param name="year"></param>
        /// <param name="cartype"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public DataTable GetLogDetail(string year, string cartype, string status)
        {
            string sql = string.Format(
                @"select log.id,log.STATUS,log.type,log.createdate,log.carno,log.address,log.DRIVERNAME,log.PHONE,log.DRIVERID from bis_carinlog log
                where 1=1
                ");
            if (!string.IsNullOrEmpty(year))
            {
                string[] arrDate = year.Split('|');
                sql += string.Format(" and (log.createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and log.createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(arrDate[0]).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(arrDate[1]).ToString("yyyy-MM-dd 23:59:59"));
            }

            if (!string.IsNullOrEmpty(cartype))
            {
                sql += string.Format(" and log.type={0}", cartype);
            }

            if (!string.IsNullOrEmpty(status))
            {
                sql += string.Format(" and log.status={0}", status);
            }

            sql += "  order by type asc,createdate desc";

            DataTable dt = this.BaseRepository().FindTable(sql);

            return dt;
        }

        /// <summary>
        /// 返回当前场内车辆数量
        /// </summary>
        /// <returns></returns>
        public int GetLogNum()
        {
            string sql = string.Format(@"select Count(ID) as num from bis_carinlog where status=0 and isleave=0 and (address='一号岗' or address='三号岗')");
            return Convert.ToInt32(BaseRepository().FindObject(sql));
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 添加通过记录
        /// </summary>
        /// <param name="carlog"></param>
        public void AddPassLog(CarinlogEntity carlog)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                //如果是班车则关联进行人员进厂记录操作
                if (carlog.Type == 0)
                {
                    //将该车1小时内的打卡记录都变为已进场
                    Repository<CarrideEntity> inlogdb = new Repository<CarrideEntity>(DbFactory.Base());
                    string sql =
                        string.Format(
                            "select * from BIS_CARRIDE  where CID ='{0}' and  STATUS=0 and TO_CHAR(SCANCODETIME,'yyyy-MM-dd hh24:mm:ss')>='{1}'",
                            carlog.CID, DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"));
                    List<CarrideEntity> Inlist = inlogdb.FindList(sql).ToList();
                    if (Inlist.Count > 0)
                    {
                        for (int i = 0; i < Inlist.Count; i++)
                        {
                            Inlist[i].Status = 1;
                            Inlist[i].ModifyDate = DateTime.Now;
                            Inlist[i].ModifyUserId = "System";
                            Inlist[i].Lid = carlog.ID;

                        }

                        res.Update<CarrideEntity>(Inlist);
                    }
                }
                //如果是出门状态 则把之前该车牌的进门状态改为已出门
                if (carlog.Status == 1)
                {
                    string sql = string.Format(@"update bis_carinlog set isleave=1 where carno='{0}' and status=0 and isleave=0",
                        carlog.CarNo);
                    //将该车1小时内的打卡记录都变为已进场
                    res.ExecuteBySql(sql);
                }

                res.Insert<CarinlogEntity>(carlog);

                res.Commit();

            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }


        /// <summary>
        /// 通过回调添加通过记录
        /// </summary>
        /// <param name="carlog"></param>
        public void BackAddPassLog(CarinlogEntity carlog, string DeviceName, string imgUrl)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                List<HikinoutlogEntity> HikList = new List<HikinoutlogEntity>();
                HikinoutlogEntity inuser = null;
                Repository<UserEntity> Userdb = new Repository<UserEntity>(DbFactory.Base());
                List<UserEntity> userlist = Userdb.FindList("select * from base_user").ToList();
                Repository<DepartmentEntity> Deptdb = new Repository<DepartmentEntity>(DbFactory.Base());
                List<DepartmentEntity> Deptlist = Deptdb.FindList("select * from base_department").ToList();
                //如果是班车则关联进行人员进厂记录操作
                if (carlog.Type == 0)
                {
                    //将该车1小时内的打卡记录都变为已进场
                    Repository<CarrideEntity> inlogdb = new Repository<CarrideEntity>(DbFactory.Base());
                    string sql =
                        string.Format(
                            "select * from BIS_CARRIDE  where CID ='{0}' and  STATUS=0 and TO_CHAR(SCANCODETIME,'yyyy-MM-dd hh24:mm:ss')>='{1}'",
                            carlog.CID, DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"));
                    List<CarrideEntity> Inlist = inlogdb.FindList(sql).ToList();
                    if (Inlist.Count > 0)
                    {
                        for (int i = 0; i < Inlist.Count; i++)
                        {
                            Inlist[i].Status = 1;
                            Inlist[i].ModifyDate = carlog.CreateDate;
                            Inlist[i].ModifyUserId = "System";
                            Inlist[i].Lid = carlog.ID;

                            UserEntity user = userlist.Where(it => it.UserId == Inlist[i].CreateUserId).FirstOrDefault();
                            if (user != null)
                            {
                                var dept = Deptlist.Where(it => it.DepartmentId == user.DepartmentId).FirstOrDefault();
                                //增加海康设备出入表记录
                                HikinoutlogEntity Hik = new HikinoutlogEntity();
                                Hik.AreaName = carlog.Address;



                                Hik.DeptId = user.DepartmentId;
                                if (dept != null)
                                {
                                    Hik.DeptName = dept.FullName;
                                }
                                else
                                {
                                    Hik.DeptName = "";
                                }


                                Hik.CreateDate = carlog.CreateDate;
                                Hik.CreateUserId = "System";
                                Hik.CreateUserDeptCode = "00";
                                Hik.CreateUserOrgCode = "00";
                                Hik.DeviceName = DeviceName;
                                Hik.DeviceType = 1;
                                Hik.EventType = 2;
                                Hik.ID = Guid.NewGuid().ToString();
                                Hik.InOut = carlog.Status;
                                Hik.UserId = user.UserId;
                                Hik.UserName = user.RealName;
                                Hik.UserType = 0;
                                Hik.ScreenShot = imgUrl;
                                Hik.IsOut = 0;
                                if (carlog.Status == 1)
                                {
                                    string Hikinoutsql = "select * from BIS_HIKINOUTLOG where userid='" + Hik.UserId + "' and InOut=0 order by createdate desc";
                                    Repository<HikinoutlogEntity> Hikinoutlog = new Repository<HikinoutlogEntity>(DbFactory.Base());

                                    //如果是出门则要先找到该人员对应的入场记录进行更改
                                    inuser = Hikinoutlog.FindList(Hikinoutsql).FirstOrDefault();
                                    if (inuser != null)
                                    {
                                        inuser.IsOut = 1;
                                        inuser.InId = Hik.ID;
                                        inuser.OutTime = carlog.CreateDate;
                                        inuser.ModifyDate = carlog.CreateDate;
                                        inuser.ModifyUserId = "System";
                                    }
                                }
                                if (inuser != null)
                                {
                                    res.Update<HikinoutlogEntity>(inuser);
                                }

                                res.Insert<HikinoutlogEntity>(Hik);
                            }


                        }

                        res.Update<CarrideEntity>(Inlist);
                    }
                }
                else if (carlog.Type == 1)
                {
                    UserEntity user = userlist.Where(it => it.UserId == carlog.DriverID).FirstOrDefault();
                    if (user != null)
                    {
                        var dept = Deptlist.Where(it => it.DepartmentId == user.DepartmentId).FirstOrDefault();
                        //增加海康设备出入表记录
                        HikinoutlogEntity Hik = new HikinoutlogEntity();
                        Hik.AreaName = carlog.Address;            
                        Hik.DeptId = user.DepartmentId;
                        if (dept != null)
                        {
                            Hik.DeptName = dept.FullName;
                        }
                        else
                        {
                            Hik.DeptName = "";
                        }         
                        Hik.CreateDate = carlog.CreateDate;
                        Hik.CreateUserId = "System";
                        Hik.CreateUserDeptCode = "00";
                        Hik.CreateUserOrgCode = "00";
                        Hik.DeviceName = DeviceName;
                        Hik.DeviceType = 1;
                        Hik.EventType = 2;
                        Hik.ID = Guid.NewGuid().ToString();
                        Hik.InOut = carlog.Status;
                        Hik.UserId = user.UserId;
                        Hik.UserName = user.RealName;
                        Hik.UserType = 0;
                        Hik.ScreenShot = imgUrl;
                        Hik.IsOut = 0;
                        if (carlog.Status == 1)
                        {
                            string Hikinoutsql = "select * from BIS_HIKINOUTLOG where userid='" + Hik.UserId + "' and InOut=0 order by createdate desc";
                            Repository<HikinoutlogEntity> Hikinoutlog = new Repository<HikinoutlogEntity>(DbFactory.Base());

                            //如果是出门则要先找到该人员对应的入场记录进行更改
                            inuser = Hikinoutlog.FindList(Hikinoutsql).FirstOrDefault();
                            if (inuser != null)
                            {
                                inuser.IsOut = 1;
                                Hik.IsOut = 1;
                                inuser.InId = Hik.ID;
                                inuser.OutTime = carlog.CreateDate;
                                inuser.ModifyDate = carlog.CreateDate;
                                inuser.ModifyUserId = "System";
                            }
                        }
                        if (inuser != null)
                        {
                            res.Update<HikinoutlogEntity>(inuser);
                        }

                        res.Insert<HikinoutlogEntity>(Hik);
                    }


                }
                else if (carlog.Type == 3)
                {
                    Repository<CarUserFileImgEntity> visitlog = new Repository<CarUserFileImgEntity>(DbFactory.Base());
                    string visitsql = "select * from bis_usercarfileimg where baseid='" + carlog.CID + "'";
                    List<CarUserFileImgEntity> visList = visitlog.FindList(visitsql).ToList();
                    if (visList.Count > 0)
                    {
                        foreach (var vist in visList)
                        {

                            //增加海康设备出入表记录
                            HikinoutlogEntity Hik = new HikinoutlogEntity();
                            Hik.AreaName = carlog.Address;
                            Hik.DeptId = "";
                            Hik.DeptName = "拜访人员";
                            Hik.CreateDate = carlog.CreateDate;
                            Hik.CreateUserId = "System";
                            Hik.CreateUserDeptCode = "00";
                            Hik.CreateUserOrgCode = "00";
                            Hik.DeviceName = DeviceName;
                            Hik.DeviceType = 1;
                            Hik.EventType = 2;
                            Hik.ID = Guid.NewGuid().ToString();
                            Hik.InOut = carlog.Status;
                            Hik.UserId = vist.ID;
                            Hik.UserName = vist.Username;
                            Hik.UserType = 2;
                            Hik.ScreenShot = imgUrl;
                            Hik.IsOut = 0;
                            if (carlog.Status == 1)
                            {
                                string Hikinoutsql = "select * from BIS_HIKINOUTLOG where userid='" + Hik.UserId + "' and InOut=0 order by createdate desc";
                                Repository<HikinoutlogEntity> Hikinoutlog = new Repository<HikinoutlogEntity>(DbFactory.Base());

                                //如果是出门则要先找到该人员对应的入场记录进行更改
                                inuser = Hikinoutlog.FindList(Hikinoutsql).FirstOrDefault();
                                if (inuser != null)
                                {
                                    inuser.IsOut = 1;
                                    Hik.IsOut = 1;
                                    inuser.InId = Hik.ID;
                                    inuser.OutTime = carlog.CreateDate;
                                    inuser.ModifyDate = carlog.CreateDate;
                                    inuser.ModifyUserId = "System";
                                }
                            }

                            if (inuser != null)
                            {
                                res.Update<HikinoutlogEntity>(inuser);
                            }

                            res.Insert<HikinoutlogEntity>(Hik);

                        }
                    }

                }
                else if (carlog.Type == 4)
                {
                    //增加海康设备出入表记录
                    HikinoutlogEntity Hik = new HikinoutlogEntity();
                    Hik.AreaName = carlog.Address;
                    Hik.DeptId = "";
                    Hik.DeptName = "物料人员";
                    Hik.CreateDate = carlog.CreateDate;
                    Hik.CreateUserId = "System";
                    Hik.CreateUserDeptCode = "00";
                    Hik.CreateUserOrgCode = "00";
                    Hik.DeviceName = DeviceName;
                    Hik.DeviceType = 1;
                    Hik.EventType = 2;
                    Hik.ID = Guid.NewGuid().ToString();
                    Hik.InOut = carlog.Status;
                    Hik.UserId = carlog.ID;
                    Hik.UserName = carlog.DriverName;
                    Hik.UserType = 2;
                    Hik.ScreenShot = imgUrl;
                    Hik.IsOut = 0;
                    if (carlog.Status == 1)
                    {
                        string Hikinoutsql =
                            "select * from BIS_HIKINOUTLOG where userid='" + Hik.UserId + "' and InOut=0 order by createdate desc";
                        Repository<HikinoutlogEntity> Hikinoutlog =
                            new Repository<HikinoutlogEntity>(DbFactory.Base());

                        //如果是出门则要先找到该人员对应的入场记录进行更改
                        inuser = Hikinoutlog.FindList(Hikinoutsql).FirstOrDefault();
                        if (inuser != null)
                        {
                            inuser.IsOut = 1;
                            Hik.IsOut = 1;
                            inuser.InId = Hik.ID;
                            inuser.OutTime = carlog.CreateDate;
                            inuser.ModifyDate = carlog.CreateDate;
                            inuser.ModifyUserId = "System";
                        }
                    }

                    if (inuser != null)
                    {
                        res.Update<HikinoutlogEntity>(inuser);
                    }

                    res.Insert<HikinoutlogEntity>(Hik);
                }
                else if (carlog.Type == 5)
                {
                    //危化品车辆 为司机添加进出场记录
                    //增加海康设备出入表记录
                    HikinoutlogEntity Hik = new HikinoutlogEntity();
                    Hik.AreaName = carlog.Address;
                    Hik.DeptId = "";
                    Hik.DeptName = "危化品人员";
                    Hik.CreateDate = carlog.CreateDate;
                    Hik.CreateUserId = "System";
                    Hik.CreateUserDeptCode = "00";
                    Hik.CreateUserOrgCode = "00";
                    Hik.DeviceName = DeviceName;
                    Hik.DeviceType = 1;
                    Hik.EventType = 2;
                    Hik.ID = Guid.NewGuid().ToString();
                    Hik.InOut = carlog.Status;
                    Hik.UserId = carlog.ID;
                    Hik.UserName = carlog.DriverName;
                    Hik.UserType = 2;
                    Hik.ScreenShot = imgUrl;
                    Hik.IsOut = 0;
                    if (carlog.Status == 1)
                    {
                        string Hikinoutsql =
                            "select * from BIS_HIKINOUTLOG where userid='" + Hik.UserId + "' and InOut=0 order by createdate desc";
                        Repository<HikinoutlogEntity> Hikinoutlog =
                            new Repository<HikinoutlogEntity>(DbFactory.Base());

                        //如果是出门则要先找到该人员对应的入场记录进行更改
                        inuser = Hikinoutlog.FindList(Hikinoutsql).FirstOrDefault();
                        if (inuser != null)
                        {
                            inuser.IsOut = 1;
                            inuser.InId = Hik.ID;
                            inuser.OutTime = carlog.CreateDate;
                            inuser.ModifyDate = carlog.CreateDate;
                            inuser.ModifyUserId = "System";
                        }
                    }

                    if (inuser != null)
                    {
                        res.Update<HikinoutlogEntity>(inuser);
                    }

                    res.Insert<HikinoutlogEntity>(Hik);
                }



                //如果是出门状态 则把之前该车牌的进门状态改为已出门
                if (carlog.Status == 1)
                {
                    string sql = string.Format(@"update bis_carinlog set isleave=1 where carno='{0}' and status=0 and isleave=0",
                        carlog.CarNo);
                    //将该车1小时内的打卡记录都变为已进场
                    res.ExecuteBySql(sql);


                }

                res.Insert<CarinlogEntity>(carlog);

                res.Commit();

            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }

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
        public void SaveForm(string keyValue, CarinlogEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }

        public int[] GetCarData()
        {
            var types = new int?[] { 0, 1, 2 };
            var total1 = this.BaseRepository().IQueryable().Where(x => types.Contains(x.Type) && x.IsLeave == 0).Count();
            var total2 = this.BaseRepository().IQueryable().Where(x => x.Type == 3 && x.IsLeave == 0).Count();
            var total3 = this.BaseRepository().IQueryable().Where(x => x.Type == 4 && x.IsLeave == 0).Count();
            var total4 = this.BaseRepository().IQueryable().Where(x => x.Type == 5 && x.IsLeave == 0).Count();
            return new int[] { total1, total2, total3, total4 };
        }

        public int Insert(CarinlogEntity carlog)
        {
            return BaseRepository().Insert(carlog);
        }
        #endregion
    }
}
