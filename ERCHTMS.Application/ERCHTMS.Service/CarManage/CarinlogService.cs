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
    /// �� ��������������¼��
    /// </summary>
    public class CarinlogService : RepositoryFactory<CarinlogEntity>, CarinlogIService
    {
        #region ��ȡ����

        /// <summary>
        /// ��ҳ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //���ݳ���ɸѡ
            if (!queryParam["CID"].IsEmpty())
            {
                string CID = queryParam["CID"].ToString();

                pagination.conditionJson += string.Format(" and CID = '{0}'", CID);

            }

            //����ʱ�����ɸѡ
            if (!queryParam["Time"].IsEmpty())
            {
                string Time = queryParam["Time"].ToString();

                pagination.conditionJson += string.Format(" and TO_CHAR(CREATEDATE,'yyyy-MM-dd') = '{0}'", Time);

            }

            //����ʱ�����ɸѡ
            if (!queryParam["startTime"].IsEmpty() && !queryParam["endTime"].IsEmpty())
            {
                string startTime = queryParam["startTime"].ToString() + " 00:00:00";
                string endTime = queryParam["endTime"].ToString() + " 23:59:59";

                pagination.conditionJson +=
                    string.Format(
                        " and TO_CHAR(CREATEDATE,'yyyy-MM-dd HH24:mm:ss') >= '{0}' and  TO_CHAR(CREATEDATE,'yyyy-MM-dd HH24:mm:ss') <= '{1}' ",
                        startTime, endTime);

            }

            //���ݽ���������ɸѡ
            if (!queryParam["Status"].IsEmpty())
            {
                string Status = queryParam["Status"].ToString();

                pagination.conditionJson += string.Format(" and STATUS = {0}", Status);
            }
            //��������
            if (!queryParam["CarType"].IsEmpty())
            {
                string CarType = queryParam["CarType"].ToString();

                pagination.conditionJson += string.Format(" and log.TYPE = {0}", CarType);
            }
            //ͨ���Ÿ�
            if (!queryParam["CurrentName"].IsEmpty())
            {
                string name = queryParam["CurrentName"].ToString();
                pagination.conditionJson += string.Format(" and log.ADDRESS like '%{0}%'", name);
            }

            if (!queryParam["condition"].IsEmpty())
            {
                //���ƺ�
                if (queryParam["condition"].ToString() == "CarNo")
                {
                    string CarNo = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and log.CARNO like '%{0}%'", CarNo);
                }
                //��ʻ��
                if (queryParam["condition"].ToString() == "Dirver")
                {
                    string Dirver = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and log.DRIVERNAME like '%{0}%'", Dirver);
                }
                //�绰����
                if (queryParam["condition"].ToString() == "Phone")
                {
                    string Phone = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and log.PHONE  like '{0}%'", Phone);
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ���½�����Ϣ
        /// </summary>
        /// <param name="CarNo"></param>
        /// <returns></returns>
        public CarinlogEntity GetNewCarinLog(string CarNo)
        {
            return BaseRepository().IQueryable(it => it.CarNo == CarNo && it.IsLeave == 0 && it.Status == 0).OrderByDescending(it => it.CreateDate).FirstOrDefault();
        }

        /// <summary>
        /// ��ȡ�б�����
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<CarinlogEntity> GetList(string Cid)
        {
            return this.BaseRepository().IQueryable(it => it.CID == Cid).OrderBy(it => it.CreateDate).ToList();
        }


        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public CarinlogEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ��������ͳ��ͼ
        /// </summary>
        /// <param name="deptCode">���ű���</param>
        /// <param name="year">ͳ�����</param>
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
                        carname = "�糧�೵";
                        break;
                    case 1:
                        carname = "˽�ҳ�";
                        break;
                    case 2:
                        carname = "���񹫳�";
                        break;
                    case 3:
                        carname = "�ݷó���";
                        break;
                    case 4:
                        carname = "���ϳ���";
                        break;
                    case 5:
                        carname = "Σ��Ʒ����";
                        break;
                }
                object[] arr = { carname, Convert.ToInt32(dt.Rows[i][1]) };
                list.Add(arr);

            }
            dt.Dispose();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// ��ȡ�������볡��Ϣ
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
        /// ���ص�ǰ���ڳ�������
        /// </summary>
        /// <returns></returns>
        public int GetLogNum()
        {
            string sql = string.Format(@"select Count(ID) as num from bis_carinlog where status=0 and isleave=0 and (address='һ�Ÿ�' or address='���Ÿ�')");
            return Convert.ToInt32(BaseRepository().FindObject(sql));
        }

        #endregion

        #region �ύ����

        /// <summary>
        /// ���ͨ����¼
        /// </summary>
        /// <param name="carlog"></param>
        public void AddPassLog(CarinlogEntity carlog)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                //����ǰ೵�����������Ա������¼����
                if (carlog.Type == 0)
                {
                    //���ó�1Сʱ�ڵĴ򿨼�¼����Ϊ�ѽ���
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
                //����ǳ���״̬ ���֮ǰ�ó��ƵĽ���״̬��Ϊ�ѳ���
                if (carlog.Status == 1)
                {
                    string sql = string.Format(@"update bis_carinlog set isleave=1 where carno='{0}' and status=0 and isleave=0",
                        carlog.CarNo);
                    //���ó�1Сʱ�ڵĴ򿨼�¼����Ϊ�ѽ���
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
        /// ͨ���ص����ͨ����¼
        /// </summary>
        /// <param name="carlog"></param>
        public void BackAddPassLog(CarinlogEntity carlog, string DeviceName, string imgUrl)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                List<HikinoutlogEntity> HikList = new List<HikinoutlogEntity>();
                HikinoutlogEntity inuser = null;
                Repository<UserEntity> Userdb = new Repository<UserEntity>(DbFactory.Base());
                List<UserEntity> userlist = Userdb.FindList("select * from base_user").ToList();
                Repository<DepartmentEntity> Deptdb = new Repository<DepartmentEntity>(DbFactory.Base());
                List<DepartmentEntity> Deptlist = Deptdb.FindList("select * from base_department").ToList();
                //����ǰ೵�����������Ա������¼����
                if (carlog.Type == 0)
                {
                    //���ó�1Сʱ�ڵĴ򿨼�¼����Ϊ�ѽ���
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
                                //���Ӻ����豸������¼
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

                                    //����ǳ�����Ҫ���ҵ�����Ա��Ӧ���볡��¼���и���
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
                        //���Ӻ����豸������¼
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

                            //����ǳ�����Ҫ���ҵ�����Ա��Ӧ���볡��¼���и���
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

                            //���Ӻ����豸������¼
                            HikinoutlogEntity Hik = new HikinoutlogEntity();
                            Hik.AreaName = carlog.Address;
                            Hik.DeptId = "";
                            Hik.DeptName = "�ݷ���Ա";
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

                                //����ǳ�����Ҫ���ҵ�����Ա��Ӧ���볡��¼���и���
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
                    //���Ӻ����豸������¼
                    HikinoutlogEntity Hik = new HikinoutlogEntity();
                    Hik.AreaName = carlog.Address;
                    Hik.DeptId = "";
                    Hik.DeptName = "������Ա";
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

                        //����ǳ�����Ҫ���ҵ�����Ա��Ӧ���볡��¼���и���
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
                    //Σ��Ʒ���� Ϊ˾����ӽ�������¼
                    //���Ӻ����豸������¼
                    HikinoutlogEntity Hik = new HikinoutlogEntity();
                    Hik.AreaName = carlog.Address;
                    Hik.DeptId = "";
                    Hik.DeptName = "Σ��Ʒ��Ա";
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

                        //����ǳ�����Ҫ���ҵ�����Ա��Ӧ���볡��¼���и���
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



                //����ǳ���״̬ ���֮ǰ�ó��ƵĽ���״̬��Ϊ�ѳ���
                if (carlog.Status == 1)
                {
                    string sql = string.Format(@"update bis_carinlog set isleave=1 where carno='{0}' and status=0 and isleave=0",
                        carlog.CarNo);
                    //���ó�1Сʱ�ڵĴ򿨼�¼����Ϊ�ѽ���
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
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
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
