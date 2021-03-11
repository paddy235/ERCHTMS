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
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.SystemManage;
using System.Text;
using ERCHTMS.Entity.LaborProtectionManage;
using System.Threading;
using System.Data.Common;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// 描 述：设备记录人员进出日志
    /// </summary>
    public class HikinoutlogService : RepositoryFactory<HikinoutlogEntity>, HikinoutlogIService
    {
        #region 获取数据
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

            //姓名
            if (!queryParam["Name"].IsEmpty())
            {
                string Name = queryParam["Name"].ToString();

                pagination.conditionJson += string.Format(" and USERNAME like '%{0}%'", Name);

            }
            //部门名称
            if (!queryParam["DeptName"].IsEmpty())
            {
                string DeptName = queryParam["DeptName"].ToString();

                pagination.conditionJson += string.Format(" and DeptName like '%{0}%'", DeptName);

            }
            //设备名称
            if (!queryParam["DeviceName"].IsEmpty())
            {
                string DeviceName = queryParam["DeviceName"].ToString();

                pagination.conditionJson += string.Format(" and DeviceName like '%{0}%'", DeviceName);

            }
            //进出类型
            if (!queryParam["OutType"].IsEmpty())
            {
                int OutType = Convert.ToInt32(queryParam["OutType"]);

                pagination.conditionJson += string.Format(" and OutType ={0}", OutType);

            }
            //设备所属门岗
            if (!queryParam["AreaName"].IsEmpty())
            {
                string AreaName = queryParam["AreaName"].ToString();
                pagination.conditionJson += string.Format(" and AreaName like '%{0}%'", AreaName);
            }
            //事件类型
            if (!queryParam["EventType"].IsEmpty())
            {
                string EventType = queryParam["EventType"].ToString();
                pagination.conditionJson += string.Format(" and EventType = '{0}'", EventType);
            }
            //开始时间
            if (!queryParam["StartTime"].IsEmpty())
            {
                string StartTime = queryParam["StartTime"].ToString();
                pagination.conditionJson += string.Format(" and CREATEDATE>=TO_DATE('{0}','yyyy-MM-dd hh24:mi:ss') ", StartTime);
            }
            //结束时间
            if (!queryParam["EndTime"].IsEmpty())
            {
                string EndTime = queryParam["EndTime"].ToString();
                pagination.conditionJson += string.Format(" and CREATEDATE<=TO_DATE('{0}','yyyy-MM-dd hh24:mi:ss')>", EndTime);
            }
            //用户类型
            if (!queryParam["UserType"].IsEmpty())
            {
                string UserType = queryParam["UserType"].ToString();
                pagination.conditionJson += string.Format(" and UserType = {0}", UserType);
            }

            //是否出厂
            if (!queryParam["Isout"].IsEmpty())
            {
                string Isout = queryParam["Isout"].ToString();
                pagination.conditionJson += string.Format(" and Isout = {0}", Isout);
            }

            //出入类型
            if (!queryParam["Inout"].IsEmpty())
            {
                string Inout = queryParam["Inout"].ToString();
                pagination.conditionJson += string.Format(" and Inout = {0}", Inout);
            }



            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取今日全厂人数
        /// </summary>
        /// <returns></returns>
        public DataTable GetNums()
        {
            string sql = string.Format(
                @"select 0 type,nvl(count(id),0) as sums,AREANAME from BIS_HIKINOUTLOG where inout=0 and isout=0 and USERTYPE=0 and createdate>=TO_DATE('{0}', 'yyyy-mm-dd HH24:mi:ss')  GROUP BY AREANAME HAVING AREANAME='三号岗'  OR  AREANAME='码头岗' 
                    union all
                  select 1 type,nvl(count(id),0) as sums,AREANAME from BIS_HIKINOUTLOG where inout=0 and isout=0 and USERTYPE=1 and createdate>=TO_DATE('{0}', 'yyyy-mm-dd HH24:mi:ss')  GROUP BY AREANAME HAVING AREANAME='三号岗'  OR  AREANAME='码头岗' 
                    union all
                  select 2 type,nvl(count(id),0) as sums,AREANAME from BIS_HIKINOUTLOG where inout=0 and isout=0 and USERTYPE=2 and createdate>=TO_DATE('{0}', 'yyyy-mm-dd HH24:mi:ss')  GROUP BY AREANAME HAVING AREANAME='三号岗'  OR  AREANAME='码头岗' 
                    union  
				  select USERTYPE as type,nvl(count(id),0) as sums,AREANAME from BIS_HIKINOUTLOG where inout=0 and isout=0 and createdate>=TO_DATE('{1}', 'yyyy-mm-dd HH24:mi:ss')  and AREANAME='一号岗' and ( USERTYPE=0 OR USERTYPE=1 OR USERTYPE=2) 
				  group by USERTYPE,AREANAME ", DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
            return this.BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// 获取当前园区内车辆
        /// </summary>
        /// <returns></returns>
        public DataTable GetCarStatistic()
        {
            string sql = string.Format(@"SELECT ADDRESS, COUNT(CARNO) CARCOUNT FROM BIS_CARINLOG WHERE TO_CHAR(CREATEDATE, 'yyyy-MM-dd') = '{0}' AND STATUS = 0 AND ISLEAVE = 0  GROUP BY ADDRESS 
                                        UNION 
                                        SELECT TO_CHAR(TYPE) AS ADDRESS,COUNT(CARNO) CARCOUNT FROM BIS_CARINLOG WHERE TO_CHAR(CREATEDATE, 'yyyy-MM-dd') = '{0}' AND STATUS = 0 AND ISLEAVE = 0  GROUP BY TYPE", DateTime.Now.ToString("yyyy-MM-dd"));
            return this.BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// 获取今日最后一次刷卡记录
        /// </summary>
        /// <returns></returns>
        public HikinoutlogEntity GetLastInoutLog()
        {
            string sql = string.Format(@"SELECT * FROM (SELECT CREATEDATE ,USERNAME,DEPTNAME,INOUT,ISOUT,AREANAME,DEVICENAME FROM BIS_HIKINOUTLOG WHERE TO_CHAR(CREATEDATE,'yyyy-mm-dd')='{0}' AND INSTR('码头岗,一号岗,三号岗',AREANAME)>0 ORDER BY CREATEDATE DESC) WHERE ROWNUM=1", DateTime.Now.ToString("yyyy-MM-dd"));
            Repository<HikinoutlogEntity> inlogdb = new Repository<HikinoutlogEntity>(DbFactory.Base());
            List<HikinoutlogEntity> list = inlogdb.FindList(sql).ToList();

            return list.OrderByDescending(x => x.CreateDate).FirstOrDefault();

        }



        /// <summary>
        /// 根据用户ID查询人员设置表
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="type">功能模块类别</param>
        /// <returns></returns>
        public bool SelSavePersonSet(string id, string type)
        {
            string sql = string.Format(@"select * from hjb_personset where userid = '{0}' and moduletype = '{1}'", id, type);

            var result = this.BaseRepository().FindTable(sql);

            return result.Rows.Count > 0;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HikinoutlogEntity> GetList(string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            StringBuilder sql = new StringBuilder("SELECT CREATEDATE ,USERNAME,DEPTNAME,INOUT,ISOUT,AREANAME,DEVICENAME FROM BIS_HIKINOUTLOG WHERE 1=1 ");
            //人员ID
            if (!queryParam["UserId"].IsEmpty())
            {
                string userId = queryParam["UserId"].ToString();
                sql.AppendFormat(" AND USERID = '{0}'", userId);
            }
            //开始时间
            if (!queryParam["StartDate"].IsEmpty())
            {
                string StartDate = queryParam["StartDate"].ToString();
                sql.AppendFormat(" AND  TO_CHAR(CREATEDATE,'yyyy-MM-dd')>='{0}'", StartDate);
            }
            //结束时间
            if (!queryParam["EndDate"].IsEmpty())
            {
                string EndDate = queryParam["EndDate"].ToString();
                DateTime tempDt = Convert.ToDateTime(EndDate).AddDays(1);
                sql.AppendFormat(" AND TO_CHAR(CREATEDATE,'yyyy-MM-dd') <'{0}'", tempDt);
            }
            sql.Append(" ORDER BY CREATEDATE DESC");
            return this.BaseRepository().FindList(sql.ToString());
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HikinoutlogEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 根据用户id查询该用户进场未出厂记录
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public HikinoutlogEntity GetInUser(string UserId)
        {

            var entity = this.BaseRepository().IQueryable(x => x.UserId == UserId && x.InOut == 0).OrderByDescending(x => x.CreateDate).FirstOrDefault();
            return entity;
            //string sql = "select * from BIS_HIKINOUTLOG where userid='" + UserId + "' and InOut=0 order by createdate desc";
            //Repository<HikinoutlogEntity> inlogdb = new Repository<HikinoutlogEntity>(DbFactory.Base());
            //List<HikinoutlogEntity> old = inlogdb.FindList(sql).ToList();
            //return old[0];
        }

        /// <summary>
        /// 根据设备ID
        /// </summary>
        /// <param name="HikId"></param>
        /// <returns></returns>
        public HikinoutlogEntity DeviceGetLog(string HikId)
        {
            string sql = string.Format("select * from BIS_HIKINOUTLOG  where devicehikid = '{0}' and inout=0 and isout=0 order by createdate asc", HikId);
            return this.BaseRepository().FindList(sql).FirstOrDefault();
        }

        /// <summary>
        /// 获取各个一级区域人数
        /// </summary>
        /// <returns></returns>
        public List<AreaModel> GetAccPersonNum()
        {
            string sql = @"select districtid,parentid,districtcode,districtname,sortcode,belongdept,createdate,createuserid,createusername,modifydate,modifyuserid,modifyusername
,deptchargeperson,disreictchargeperson,linktel,belongcompany,linkman,linkemail,chargedept,createuserdeptcode,createuserorgcode,deptchargepersonid
,disreictchargepersonid,chargedeptid,linktocompany,linktocompanyid,chargedeptcode,organizeid,latlng,areaid,nvl(numb,0) numb from bis_district dis
                            left join(select areaid, sum(numb) numb from (
                            select ID, AREAID, nvl(cou,0) numb from BIS_HIKACCESS acc
                            left join(select devicehikid, count(devicehikid) cou from bis_hikinoutlog where devicetype = 2 and isout = 0  group by devicehikid) log on acc.hikid = log.devicehikid
                            ) group by areaid ) acclog on dis.districtid = acclog.areaid where parentid='0'";
            Repository<AreaModel> inlogdb = new Repository<AreaModel>(DbFactory.Base());
            List<AreaModel> AreaList = inlogdb.FindList(sql).ToList();
            List<AreaModel> AreaParent = AreaList.Where(it => it.ParentID == "0").ToList();
            for (int i = 0; i < AreaParent.Count; i++)
            {
                int num = AreaParent[i].Numb;
                num += GetSonNum(AreaList, AreaParent[i].DistrictID);
                AreaParent[i].Numb = num;
            }

            return AreaParent;
        }

        /// <summary>
        /// 获取父节点Code获取其下所有子节点
        /// </summary>
        /// <returns></returns>
        public List<DistrictEntity> GetAreaSon(string code)
        {
            string sql = string.Format("select * from bis_district where districtcode like '{0}%' order by districtcode", code);
            Repository<DistrictEntity> inlogdb = new Repository<DistrictEntity>(DbFactory.Base());
            List<DistrictEntity> AreaList = inlogdb.FindList(sql).ToList();
            return AreaList;
        }

        /// <summary>
        /// 递归求和
        /// </summary>
        /// <param name="AreaList"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public int GetSonNum(List<AreaModel> AreaList, string parentid)
        {
            int sonNum = 0;
            List<AreaModel> SonList = AreaList.Where(it => it.ParentID == parentid).ToList();
            if (SonList != null && SonList.Count > 0)
            {
                foreach (var item in SonList)
                {
                    sonNum += item.Numb;
                    GetSonNum(AreaList, item.DistrictID);
                }
            }
            return sonNum;
        }


        /// <summary>
        /// 根据hikid设备的ID获取人员进出的前五条数据
        /// </summary>
        /// <param name="hikId">设备的Id</param>
        /// <returns></returns>
        public System.Collections.IList GetTopFiveById(string hikId)
        {
            return this.BaseRepository().IQueryable(x => x.DeviceHikID.Equals(hikId)).OrderByDescending(x => x.CreateDate).Select(x => new
            {
                x.CreateDate,
                x.UserName,
                x.DeptName,
                InOut = x.InOut == 0 ? "进门" : "出门",
                x.ScreenShot
            }).Take(5).ToList();

        }

        /// <summary>
        /// 获取设备间监控第一条数据
        /// </summary>
        /// <returns></returns>
        public HikinoutlogEntity GetFirsetData()
        {
            return this.BaseRepository().IQueryable(x => x.DeviceType == 2).OrderByDescending(x => x.CreateDate).FirstOrDefault();
        }

        /// <summary>
        /// 根据门禁点设备编号获取监控编号
        /// </summary>
        /// <param name="DoorIndexCode">门禁点设备编号</param>
        /// <returns></returns>
        public string GetCameraIndexCodeByDoorIndexCode(string DoorIndexCode)
        {
            string CameraIndexCode = string.Empty;
            string sql = string.Format("SELECT CAMERAINDEXCODE FROM BIS_HIK_DEV_CAMERAMAP WHERE IS_DEL=0 AND DOORINDEXCODE='{0}'", DoorIndexCode);

            DataTable dt = this.BaseRepository().FindTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                CameraIndexCode = dt.Rows[0][0].ToString();
            }
            return CameraIndexCode;
        }

        /// <summary>
        /// 考勤告警
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetAttendanceWarningPageList(Pagination pagination, string queryJson)
        {
            StringBuilder sb = new StringBuilder(@"SELECT  v.FULLNAME,v.DEPARTMENTID,v.DEPARTMENTCODE, v.REALNAME,v.DUTYID,v.DUTYNAME,
                                                                        v.REMARK ,v.DEPTTYPE,v.dkremark,v.DEPTSORT , v.SORTCODE,1 as personcount FROM V_ATTENDANCEWARNING v
                                                                        WHERE v.DEPARTMENTID not in ('0') ");
            var param = new List<DbParameter>();

            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
          

            var queryParam = queryJson.ToJObject();

            //姓名
            if (!queryParam["RealName"].IsEmpty())
            {
                string Name = queryParam["RealName"].ToString();
                sb.AppendFormat(" and v.REALNAME like @RealName");
                param.Add(DbParameters.CreateDbParameter("@RealName", '%' + Name + '%'));
                //pagination.conditionJson += string.Format(" and v.REALNAME like '%{0}%'", Name);
            }
            //部门类型
            if (!queryParam["DeptType"].IsEmpty())
            {
                string DeptType = queryParam["DeptType"].ToString();
                sb.AppendFormat(" and v.DEPTTYPE= @DeptType");
                param.Add(DbParameters.CreateDbParameter("@DeptType", DeptType));
                //pagination.conditionJson += string.Format(" and v.DEPTTYPE= '{0}'", DeptType);
            }
            //内部或者外包单位
            if (!queryParam["isinorout"].IsEmpty())
            {
                int isinorout = Convert.ToInt32(queryParam["isinorout"]);
                if (isinorout == 1)
                {
                    //pagination.conditionJson += string.Format(" and v.DEPTTYPE is null ");
                    sb.AppendFormat(" and v.DEPTTYPE is null");
                    pagination.sidx = string.Format(" DEPTSORTss,v.DEPTSORT asc,v.deptcode asc,v.userid desc,v.REALNAME ");
                    pagination.sord = "asc";
                }
                if (isinorout == 0)
                {
                    // pagination.conditionJson += string.Format(" and v.DEPTTYPE is not null");
                    sb.AppendFormat(" and v.DEPTTYPE is not null");
                    pagination.sidx = string.Format("v.DEPTTYPE,v.DEPTSORT,v.DEPTNAME,v.DEPTCODE,v.USERID desc,v.REALNAME");
                    pagination.sord = "asc";
                }
            }
            if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string deptCode = queryParam["code"].ToString();
                string deptType = "";
                if (deptCode.StartsWith("cx100_"))
                {
                    deptCode = deptCode.Replace("cx100_", "");
                    deptType = "长协";
                }
                if (deptCode.StartsWith("ls100_"))
                {
                    deptCode = deptCode.Replace("ls100_", "");
                    deptType = "临时";
                }
                if (queryParam["isOrg"].ToString() == "Organize")
                {
                    //pagination.conditionJson += string.Format(" and v.organizecode='{0}'", deptCode);
                    sb.AppendFormat(" and v.organizecode=@organizecode");
                    param.Add(DbParameters.CreateDbParameter("@organizecode", deptCode));
                }

                else if (queryParam["isOrg"].ToString() == "Department" || user.IsSystem)
                {

                    //pagination.conditionJson += string.Format(" and (v.departmentcode like '{0}%' or v.nickname  in (select departmentid from base_department where encode like '{0}%'))", deptCode);
                    sb.AppendFormat(" and (v.departmentcode like @deptCode1 or v.nickname  in (select departmentid from base_department where encode like @deptCode2))");
                    param.Add(DbParameters.CreateDbParameter("@deptCode1", deptCode + '%'));
                    param.Add(DbParameters.CreateDbParameter("@deptCode2", deptCode + '%'));
                }
                else
                {
                    //pagination.conditionJson += string.Format(" and (v.departmentcode='{0}' or v.nickname in (select departmentid from base_department where encode='{0}'))", deptCode);
                    sb.AppendFormat(" and (v.departmentcode=@departmentcode or v.nickname in (select departmentid from base_department where encode=@encode1))");
                    param.Add(DbParameters.CreateDbParameter("@departmentcode", deptCode));
                    param.Add(DbParameters.CreateDbParameter("@encode1", deptCode));
                }

                if (!string.IsNullOrWhiteSpace(deptType))
                {
                    //pagination.conditionJson += string.Format("and v.departmentid in(select departmentid from base_department d where d.depttype='{0}' and d.encode like '{1}%')", deptType, deptCode);
                    sb.AppendFormat(" and v.departmentid in(select departmentid from base_department d where d.depttype=@deptType1 and d.encode like @encode2)");
                    param.Add(DbParameters.CreateDbParameter("@deptType1", deptType));
                    param.Add(DbParameters.CreateDbParameter("@encode2", deptCode + '%'));
                }
            }
            //判断当前登陆用户是什么级别
            if (!curuser.RoleName.IsEmpty())
            {
                string RoleName = curuser.RoleName.ToString();
                string accounts = this.BaseRepository().FindTable("SELECT ITEMVALUE FROM BASE_DATAITEMDETAIL WHERE ITEMNAME='SpecialAccount'").Rows[0]["ITEMVALUE"].ToString();

                int IsAppointAccount = 0;
                if (!string.IsNullOrEmpty(accounts))//判断特定权限
                {
                    List<string> accountArray = accounts.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    Operator user = OperatorProvider.Provider.Current();
                    if (accountArray.Contains(user.Account.ToLower()))
                        IsAppointAccount = 1;
                }
                if (IsAppointAccount != 1)
                {

                    if (!curuser.RoleName.Contains("厂级部门用户") && !curuser.RoleName.Contains("公司级用户"))
                    {
                        StringBuilder sqluser = new StringBuilder(@"SELECT USERID FROM HJB_PERSONSET WHERE MODULETYPE=2 AND USERID=@PERSONSETUserId");
                        //string sqluser = string.Format("SELECT USERID FROM HJB_PERSONSET WHERE MODULETYPE=2 AND USERID='{0}' ", curuser.UserId);
                        var param2 = new List<DbParameter>();
                        param2.Add(DbParameters.CreateDbParameter("@PERSONSETUserId", curuser.UserId));
                        DataTable dtperson = this.BaseRepository().FindTable(sqluser.ToString(), param2.ToArray());
                        if (dtperson != null && dtperson.Rows.Count > 0)//当前用户是否为设置表里的用户- 自己看自己
                        {
                            //pagination.conditionJson += string.Format(" and   v.USERID not in (SELECT USERID FROM HJB_PERSONSET WHERE MODULETYPE = 2  AND USERID<>'{0}') ", curuser.UserId);
                            sb.AppendFormat(" and   v.USERID not in (SELECT USERID FROM HJB_PERSONSET WHERE MODULETYPE = 2  AND USERID<>@UserId) ");
                            param.Add(DbParameters.CreateDbParameter("@UserId", curuser.UserId));
                        }
                        else//不是特定用户也不是设置人员-都不能看到设置人员
                        {
                            //pagination.conditionJson += string.Format(" and   v.USERID not in (SELECT USERID FROM HJB_PERSONSET WHERE MODULETYPE = 2 ) ");
                            sb.AppendFormat(" and   v.USERID not in (SELECT USERID FROM HJB_PERSONSET WHERE MODULETYPE = 2 ) ");
                        }

                    }
                    if (RoleName.Contains("承包商级用户"))
                    {
                        //承包商级用户只可查看本单位门禁数据
                        var bmname = GetBmname(curuser.UserId);

                        //pagination.conditionJson += string.Format(" and v.ROLENAME like '%{0}%' and v.DEPTNAME = '{1}'", RoleName, bmname);
                        sb.AppendFormat(" and v.ROLENAME like @RoleName and v.DEPTNAME = @bmname");
                        param.Add(DbParameters.CreateDbParameter("@RoleName", '%' + RoleName + '%'));
                        param.Add(DbParameters.CreateDbParameter("@bmname", bmname));
                    }
                    else if (RoleName.Contains("厂级部门用户") || RoleName.Contains("公司管理员") || RoleName.Contains("公司级用户") || RoleName.Contains("超级管理员"))
                    {
                        //此级别的用户可查看所有数据
                    }
                    else
                    {
                        var bmname = GetBmname(curuser.UserId);

                        //pagination.conditionJson += string.Format(" and (v.ROLENAME like '%承包商级用户%' or v.DEPTNAME = '{0}' or v.PARENTNAME = '{0}')", bmname);
                        sb.AppendFormat(" and (v.ROLENAME like '%承包商级用户%' or v.DEPTNAME = @bmname or v.PARENTNAME = @bmname)");
                        param.Add(DbParameters.CreateDbParameter("@bmname", bmname));
                    }
                }
            }
            //pagination.sord = "desc";
            //pagination.sidx = "v.DEPTSORT asc, v.SORTCODE asc ,v.USERID";
            DateTime DKStartTime = Convert.ToDateTime(queryParam["StartTime"].ToString());//开始日期
            DateTime DKEndTime = Convert.ToDateTime(queryParam["EndTime"].ToString());//结束日期
            int DayCount = Convert.ToInt32(queryParam["DayCount"]);//要查询的连续未打卡天数


            int rows = pagination.rows;
            int page = pagination.page;

            pagination.page = 1;
            pagination.rows = 1000000000;

            DataTable result = this.BaseRepository().FindTable(sb.ToString(), param.ToArray(), pagination);
            //DataTable result = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            for (int i = 0; i < result.Rows.Count; i++)
            {
                DataRow item = result.Rows[i];
                string remark = item["remark"].ToString();//打卡日期集合（逗号隔开）
                string[] dateArry = new string[0];//打卡日期数组集合
                if (!string.IsNullOrEmpty(remark))
                    dateArry = remark.Split(',');
                int daydk = 0;//未打卡次数
                string dkremark = "";//返回前端的连续未打卡信息
                DateTime sdate = new DateTime();//定义一个连续未打卡的时间段的开始日期
                for (DateTime d = DKStartTime; d <= DKEndTime; d = d.AddDays(1))//循环开始结束日期
                {
                    if (dateArry.FirstOrDefault(t => t.Contains(d.ToString("yyyy-MM-dd"))) == null)//判断是否存在该日期的打卡信息
                    {
                        daydk++;//累加天数
                        if (sdate < DKStartTime)
                            sdate = d;//赋值开始日期
                        if (daydk == DayCount)
                        {
                            dkremark += string.Format(" {0} 至{1}连续{2}天缺勤;", sdate.ToString("yyyy-MM-dd"), d.ToString("yyyy-MM-dd"), DayCount);
                            daydk = 0;//初始化连续天数
                            sdate = new DateTime();//初始化赋值开始日期
                        }
                    }
                    else
                    {
                        daydk = 0;//初始化连续天数
                        sdate = new DateTime();//初始化赋值开始日期
                    }
                }
                item["dkremark"] = dkremark;
                if (dkremark == "")
                    //result.Rows.RemoveAt(i);
                    result.Rows[i].Delete();
            }
            result.AcceptChanges();
            //foreach (DataRow item in result.Rows)
            //{
            //    string remark = item["remark"].ToString();//打卡日期集合（逗号隔开）
            //    string[] dateArry = new string[0];//打卡日期数组集合
            //    if (!string.IsNullOrEmpty(remark))
            //        dateArry = remark.Split(',');
            //    int daydk = 0;//未打卡次数
            //    string dkremark = "";//返回前端的连续未打卡信息
            //    DateTime sdate = new DateTime();//定义一个连续未打卡的时间段的开始日期
            //    for (DateTime d = DKStartTime; d <= DKEndTime; d = d.AddDays(1))//循环开始结束日期
            //    {
            //        if (dateArry.FirstOrDefault(t => t.Contains(d.ToString("yyyy-MM-dd"))) == null)//判断是否存在该日期的打卡信息
            //        {
            //            daydk++;//累加天数
            //            if (sdate < DKStartTime)
            //                sdate = d;//赋值开始日期
            //            if (daydk == DayCount)
            //            {
            //                dkremark += string.Format(" {0} 至{1}连续{2}天缺勤;", sdate.ToString("yyyy-MM-dd"), d.ToString("yyyy-MM-dd"), DayCount);
            //                daydk = 0;//初始化连续天数
            //                sdate = new DateTime();//初始化赋值开始日期
            //            }
            //        }
            //        else
            //        {
            //            daydk = 0;//初始化连续天数
            //            sdate = new DateTime();//初始化赋值开始日期
            //        }
            //    }
            //    item["dkremark"] = dkremark;
            //    if (dkremark == "")
            //        result.Rows.RemoveAt(item.i);
            //}
            //if (result.Rows.Count==0)
            //{
            //    return result;
            //}

            //DataTable newdt = result.Clone();
            //newdt.Clear();

            //var Datarows = result.Rows.Cast<DataRow>();
            ////.OrderBy(t=>t["DEPTSORT"].ToString()).OrderBy(t=>t["SORTCODE"].ToString()).OrderByDescending(t=>t["userid"].ToString())
            //var curDataRows = Datarows.Skip(page-1).Take(rows).ToArray();
            //foreach (var item in curDataRows)
            //{
            //    newdt.ImportRow(item);
            //}
            pagination.records = result.Rows.Count;
            pagination.rows = rows;
            pagination.page = page;
            //return newdt;
            return GetPageDataTable(result, page, rows);
        }

        public DataTable GetPageDataTable(DataTable dt, int PageIndex, int PageSize)
        {
            if (PageIndex == 0)
                return dt;
            DataTable newdt = dt.Copy();
            newdt.Clear();
            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;
            if (rowbegin >= dt.Rows.Count)
                return newdt;
            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            DataTable dtgroupBy = dt.AsEnumerable().GroupBy(r => new
            {
                fullname = r["fullname"]
            }).Select(g =>
            {
                var row = dt.NewRow();

                row["fullname"] = g.Key.fullname;
                row["personcount"] = g.Sum(r => (decimal)r["personcount"]);
                return row;
            }).CopyToDataTable();
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                //string fitter = string.Format("fullname='{0}'", dr["fullname"]);
                //int count = dt.Select(fitter) == null ? 0 : dt.Select(fitter).Count();
                //dr["personcount"] = count;
                string fitter = string.Format("fullname='{0}'", dr["fullname"]);
                DataRow drgroupBy = dtgroupBy.Select(fitter).FirstOrDefault();
                int count = drgroupBy == null ? 0 : drgroupBy["personcount"].ToInt();
                dr["personcount"] = count;
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }

        /// <summary>
        /// 连续缺勤统计
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetAbsenteeismPageList(Pagination pagination, string queryJson)
        {
//            StringBuilder sb = new StringBuilder(@"SELECT u.USERID, 
//u.DEPARTMENTID,u.DEPARTMENTCODE, u.REALNAME,u.DUTYID,u.DUTYNAME,u.depttype,
//case when u.nature = '班组' then u.parentname else u.DEPTNAME end FULLNAME,
//COUNT(u.USERID) OVER(partition by case when u.nature = '班组' then u.parentname else u.DEPTNAME end ) AS personcount,
//  case when (length(u.deptcode)>20) then (select d.SORTCODE from base_department d where d.deptcode = substr(u.deptcode,1,20)) else u.DEPTSORT end as DEPTSORTss
//FROM V_USERINFO u 
//left  JOIN  BIS_HIKINOUTLOG bh on bh.USERID = u.USERID ");
            //var param = new List<DbParameter>();
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();

            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime DKStartTime = Convert.ToDateTime(queryParam["StartTime"].ToString());//开始日期
                DateTime DKEndTime = Convert.ToDateTime(queryParam["EndTime"].ToString());//结束日期

                pagination.p_tablename += string.Format(@"  AND  TO_CHAR(bh.CREATEDATE, 'yyyy-MM-dd')>='{0}' 
                AND  TO_CHAR(bh.CREATEDATE, 'yyyy-MM-dd')<='{1}'   ", DKStartTime.ToString("yyyy-MM-dd"), DKEndTime.ToString("yyyy-MM-dd"));

                //sb.AppendFormat(@" AND  TO_CHAR(bh.CREATEDATE, 'yyyy-MM-dd')>=@DKStartTime 
                //AND  TO_CHAR(bh.CREATEDATE, 'yyyy-MM-dd') <= @DKEndTime");
                //sb.AppendFormat(@" AND  TO_CHAR(bh.CREATEDATE, 'yyyy-MM-dd')>='{0}' 
                //AND  TO_CHAR(bh.CREATEDATE, 'yyyy-MM-dd')<='{1}'   ", DKStartTime.ToString("yyyy-MM-dd"), DKEndTime.ToString("yyyy-MM-dd"));
                //param.Add(DbParameters.CreateDbParameter("@DKStartTime", DKStartTime.ToString("yyyy-MM-dd")));
                //param.Add(DbParameters.CreateDbParameter("@DKEndTime", DKEndTime.ToString("yyyy-MM-dd")));

                //---------查询条件
               // sb.AppendFormat(@" where u.DEPARTMENTID not in ('0')  AND  TO_CHAR(bh.CREATEDATE, 'yyyy-MM-dd') is NULL  AND u.USERID not in (SELECT USERID FROM HJB_PERSONSET WHERE MODULETYPE = 3)");
                //姓名
                if (!queryParam["RealName"].IsEmpty())
                {
                    string Name = queryParam["RealName"].ToString();

                    pagination.conditionJson += string.Format(" and u.REALNAME like '%{0}%'", Name);
                    //sb.AppendFormat(@"  and u.REALNAME like @RealName");
                    //sb.AppendFormat(@"  and u.REALNAME like '%{0}%'", Name);
                    //param.Add(DbParameters.CreateDbParameter("@RealName", '%' + Name + '%'));
                }
                //部门类型
                if (!queryParam["DeptType"].IsEmpty())
                {
                    string DeptType = queryParam["DeptType"].ToString();

                    pagination.conditionJson += string.Format(" and u.DEPTTYPE= '{0}'", DeptType);
                    //sb.AppendFormat(" and u.DEPTTYPE= @DeptType");
                    //sb.AppendFormat(" and u.DEPTTYPE= '{0}'", DeptType);
                    //param.Add(DbParameters.CreateDbParameter("@DeptType", DeptType));
                }
                //内部或者外包单位
                if (!queryParam["isinorout"].IsEmpty())
                {
                    int isinorout = Convert.ToInt32(queryParam["isinorout"]);
                    if (isinorout == 1)
                    {
                        pagination.conditionJson += string.Format(" and u.DEPTTYPE is null ");
                        //sb.AppendFormat(" and u.DEPTTYPE is null");
                        pagination.sidx = string.Format(" DEPTSORTss,u.DEPTSORT asc,u.deptcode asc,u.userid desc,u.REALNAME ");
                        pagination.sord = "asc";
                    }
                    if (isinorout == 0)
                    {
                        pagination.conditionJson += string.Format(" and u.DEPTTYPE is not null");
                        //sb.AppendFormat(" and u.DEPTTYPE is not null");
                        pagination.sidx = string.Format("u.DEPTTYPE,u.DEPTSORT,u.DEPTNAME,u.DEPTCODE,u.USERID desc,u.REALNAME");
                        pagination.sord = "asc";
                    }
                }
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    string deptCode = queryParam["code"].ToString();
                    string deptType = "";
                    if (deptCode.StartsWith("cx100_"))
                    {
                        deptCode = deptCode.Replace("cx100_", "");
                        deptType = "长协";
                    }
                    if (deptCode.StartsWith("ls100_"))
                    {
                        deptCode = deptCode.Replace("ls100_", "");
                        deptType = "临时";
                    }
                    if (queryParam["isOrg"].ToString() == "Organize")
                    {
                        pagination.conditionJson += string.Format(" and u.organizecode='{0}'", deptCode);
                        //sb.AppendFormat(" and u.organizecode=@organizecode");
                        //sb.AppendFormat(" and u.organizecode='{0}'", deptCode);
                        //param.Add(DbParameters.CreateDbParameter("@organizecode", deptCode));
                    }

                    else if (queryParam["isOrg"].ToString() == "Department" || user.IsSystem)
                    {
                        pagination.conditionJson += string.Format(" and (u.departmentcode like '{0}%' or u.nickname  in (select departmentid from base_department where encode like '{0}%'))", deptCode);
                        //sb.AppendFormat(" and (u.departmentcode like @deptCode1 or u.nickname  in (select departmentid from base_department where encode like @deptCode2))");
                        //sb.AppendFormat(" and (u.departmentcode like '{0}%' or u.nickname  in (select departmentid from base_department where encode like '{0}%'))", deptCode);
                        //param.Add(DbParameters.CreateDbParameter("@deptCode1", deptCode + '%'));
                       // param.Add(DbParameters.CreateDbParameter("@deptCode2", deptCode + '%'));
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and (u.departmentcode='{0}' or u.nickname in (select departmentid from base_department where encode='{0}'))", deptCode);
                        //sb.AppendFormat(" and (u.departmentcode=@departmentcode or u.nickname in (select departmentid from base_department where encode=@encode1))");
                        //sb.AppendFormat(" and (u.departmentcode='{0}' or u.nickname in (select departmentid from base_department where encode='{0}'))", deptCode);
                        //param.Add(DbParameters.CreateDbParameter("@departmentcode", deptCode));
                        //param.Add(DbParameters.CreateDbParameter("@encode1", deptCode));
                    }
                    if (!string.IsNullOrWhiteSpace(deptType))
                    {
                        pagination.conditionJson += string.Format("and u.departmentid in(select departmentid from base_department d where d.depttype='{0}' and d.encode like '{1}%')", deptType, deptCode);
                        //sb.AppendFormat(" and u.departmentid in(select departmentid from base_department d where d.depttype=@deptType1 and d.encode like @encode2)");
                        //sb.AppendFormat(" and u.departmentid in(select departmentid from base_department d where d.depttype='{0}' and d.encode like '{1}%')", deptType, deptCode);
                        //param.Add(DbParameters.CreateDbParameter("@deptType1", deptType));
                        //param.Add(DbParameters.CreateDbParameter("@encode2", deptCode + '%'));
                    }
                }
                //判断当前登陆用户是什么级别
                if (!curuser.RoleName.IsEmpty())
                {
                    string RoleName = curuser.RoleName.ToString();
                    string accounts = this.BaseRepository().FindTable("SELECT ITEMVALUE FROM BASE_DATAITEMDETAIL WHERE ITEMNAME='SpecialAccount'").Rows[0]["ITEMVALUE"].ToString();

                    int IsAppointAccount = 0;
                    if (!string.IsNullOrEmpty(accounts))
                    {
                        List<string> accountArray = accounts.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        Operator user = OperatorProvider.Provider.Current();
                        if (accountArray.Contains(user.Account.ToLower()))
                            IsAppointAccount = 1;
                    }
                    if (IsAppointAccount != 1)
                    {
                        if (RoleName.Contains("承包商级用户"))
                        {
                            //承包商级用户只可查看本单位门禁数据
                            var bmname = GetBmname(curuser.UserId);

                            pagination.conditionJson += string.Format(" and u.ROLENAME like '%{0}%' and u.DEPTNAME = '{1}'", RoleName, bmname);
                            //sb.AppendFormat(" and u.ROLENAME like @RoleName and u.DEPTNAME = @bmname");
                            //sb.AppendFormat(" and u.ROLENAME like '%{0}%' and u.DEPTNAME = '{1}'", RoleName, bmname);
                            //param.Add(DbParameters.CreateDbParameter("@RoleName", '%' + RoleName + '%'));
                            //param.Add(DbParameters.CreateDbParameter("@bmname", bmname));
                        }
                        else if (RoleName.Contains("厂级部门用户") || RoleName.Contains("公司管理员") || RoleName.Contains("公司级用户") || RoleName.Contains("超级管理员"))
                        {
                            //此级别的用户可查看所有数据
                        }
                        else
                        {
                            var bmname = GetBmname(curuser.UserId);

                            pagination.conditionJson += string.Format(" and (u.ROLENAME like '%承包商级用户%' or u.DEPTNAME = '{0}' or u.PARENTNAME = '{0}')", bmname);
                            //sb.AppendFormat(" and (u.ROLENAME like '%承包商级用户%' or u.DEPTNAME = @bmname or u.PARENTNAME = @bmname)");
                            //sb.AppendFormat(" and (u.ROLENAME like '%承包商级用户%' or u.DEPTNAME = '{0}' or u.PARENTNAME = '{0}')", bmname);
                            //param.Add(DbParameters.CreateDbParameter("@bmname", bmname));
                        }
                    }
                }

                //pagination.sord = "desc";
                //pagination.sidx = "u.DEPTSORT asc, u.SORTCODE asc ,u.USERID";
            }
           
             // DataTable result = this.BaseRepository().FindTable(sb.ToString(),param.ToArray(),pagination );
            DataTable result = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return result;
        }


        /// <summary>
        /// 获取连续缺勤统计人员设置表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="ModuleType"></param>
        /// <returns></returns>
        public DataTable GetAbsenteeismPersonSet(Pagination pagination, string ModuleType)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (!string.IsNullOrEmpty(ModuleType))
            {
                pagination.conditionJson += string.Format(" and a.MODULETYPE = '{0}'", ModuleType);
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 根据人员设置ID删除数据
        /// </summary>
        public void DeleteAbsenteeismPersonSet(string keyValue)
        {
            var res = DbFactory.Base().BeginTrans();

            try
            {
                string[] list = keyValue.Split(',');
                res.Delete<PersonSetEntity>(it => list.Contains(it.PersonSetId));
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }


        /// <summary>
        /// 批量设置新增人员不可查询门禁
        /// </summary>
        public void SaveAbsenteeismPersonSet(string json, string ModuleType)
        {
            var res = DbFactory.Base().BeginTrans();

            try
            {
                IList<PersonSetEntity> entityList = new List<PersonSetEntity>();
                string[] list = json.Split(',');

                foreach (var item in list)
                {
                    PersonSetEntity entity = new PersonSetEntity();

                    var result = SelSavePersonSet(item.ToString(), ModuleType);
                    if (result == false)
                    {
                        entity.Create();
                        entity.UserId = item.ToString();
                        entity.ModuleType = ModuleType;
                        entity.Isrefer = 0;
                        entityList.Add(entity);
                    }
                }
                res.Insert<PersonSetEntity>(entityList);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// 获取考勤告警人员设置表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="ModuleType"></param>
        /// <returns></returns>
        public DataTable GetAttendanceWarningPersonSet(Pagination pagination, string ModuleType)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (!string.IsNullOrEmpty(ModuleType))
            {
                pagination.conditionJson += string.Format(" and a.MODULETYPE = '{0}'", ModuleType);
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 根据人员设置ID删除数据
        /// </summary>
        public void DeleteAttendanceWarningPersonSet(string keyValue)
        {
            var res = DbFactory.Base().BeginTrans();

            try
            {
                string[] list = keyValue.Split(',');
                res.Delete<PersonSetEntity>(it => list.Contains(it.PersonSetId));
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }


        /// <summary>
        /// 批量设置新增人员不可查询门禁
        /// </summary>
        public void SaveAttendanceWarningPersonSet(string json, string ModuleType)
        {
            var res = DbFactory.Base().BeginTrans();

            try
            {
                IList<PersonSetEntity> entityList = new List<PersonSetEntity>();
                string[] list = json.Split(',');

                foreach (var item in list)
                {
                    PersonSetEntity entity = new PersonSetEntity();

                    var result = SelSavePersonSet(item.ToString(), ModuleType);
                    if (result == false)
                    {
                        entity.Create();
                        entity.UserId = item.ToString();
                        entity.ModuleType = ModuleType;
                        entity.Isrefer = 0;
                        entityList.Add(entity);
                    }
                }
                res.Insert<PersonSetEntity>(entityList);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// 获取各部门的在厂人员统计数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetAllDepartment(string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();

            var queryParam = queryJson.ToJObject();
            StringBuilder builder = new StringBuilder();
            var sqlparam = new List<DbParameter>();
            builder.AppendFormat(@"with table01 as(
                                     select t.USERID,t.ISREFER,substr(b.deptcode,1,20) as DEPTCODE,
                                        case when (b.ISEPIBOLY = '否' and b.DEPTTYPE is null and length(b.deptcode)>20) then  
									(select d.SORTCODE from base_department d where d.deptcode = substr(b.deptcode,1,20)) else b.DEPTSORT end as DEPTSORT,
                                        b.REALNAME,b.ISEPIBOLY,case when b.depttype is null then '内部' else b.depttype end as depttype,
										case when (b.ISEPIBOLY = '否' and b.DEPTTYPE is null and length(b.deptcode)>20) then (select fullname from base_department d where d.deptcode = substr(b.deptcode,1,20)) 
										else b.DEPTNAME end as fullname
									  from bis_hikinoutlog a
                                      left join V_USERINFO b on a.userid = b.userid
                                      left join (select * from HJB_PERSONSET where MODULETYPE = 0) t on a.userid = t.userid
                                      where b.DEPTNAME is not null and a.INOUT = 0 and not exists(select 1 from bis_hikinoutlog d where d.userid = a.userid and d.CREATEDATE+0 > a.CREATEDATE+0)");
            //姓名
            if (!queryParam["Name"].IsEmpty())
            {
                string Name = queryParam["Name"].ToString();

                //sql += string.Format(" and USERNAME like '%{0}%'", Name);

                builder.AppendFormat(@" and b.REALNAME like @Name");
                sqlparam.Add(DbParameters.CreateDbParameter("@Name", '%' + Name + '%'));
            }
            //新增部门Code查询条件
            if (!queryParam["deptCode"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string deptCode = queryParam["deptCode"].ToString();
                string deptType = "";
                if (deptCode.StartsWith("cx100_"))
                {
                    deptCode = deptCode.Replace("cx100_", "");
                    deptType = "长协";
                }
                if (deptCode.StartsWith("ls100_"))
                {
                    deptCode = deptCode.Replace("ls100_", "");
                    deptType = "临时";
                }
                if (queryParam["isOrg"].ToString() == "Organize")
                {
                    //sql += string.Format(" and b.organizecode='{0}'", deptCode);

                    builder.AppendFormat(" and b.organizecode = @organizecode");
                    sqlparam.Add(DbParameters.CreateDbParameter("@organizecode", deptCode));
                }
                else if (queryParam["isOrg"].ToString() == "Department" || user.IsSystem)
                {
                    //sql += string.Format(" and (b.departmentcode like '{0}%' or b.nickname  in (select departmentid from base_department where encode like '{0}%'))", deptCode);

                    builder.AppendFormat(" and (b.departmentcode like @departmentcode or b.nickname  in (select departmentid from base_department where encode like @encode))");
                    sqlparam.Add(DbParameters.CreateDbParameter("@departmentcode", deptCode + '%'));
                    sqlparam.Add(DbParameters.CreateDbParameter("@encode", deptCode + '%'));
                }
                else
                {
                    //sql += string.Format(" and (b.departmentcode='{0}' or b.nickname in (select departmentid from base_department where encode='{0}'))", deptCode);

                    builder.AppendFormat(" and (b.departmentcode=@departmentcode or b.nickname in (select departmentid from base_department where encode=@encode))");
                    sqlparam.Add(DbParameters.CreateDbParameter("@departmentcode", deptCode));
                    sqlparam.Add(DbParameters.CreateDbParameter("@encode", deptCode));
                }
                if (!string.IsNullOrWhiteSpace(deptType))
                {
                    //sql += string.Format(" and b.departmentid in(select departmentid from base_department d where d.depttype='{0}' and d.encode like '{1}%')", deptType, deptCode);

                    builder.AppendFormat(" and b.departmentid in(select departmentid from base_department d where d.depttype=@depttype and d.encode like @encode)");
                    sqlparam.Add(DbParameters.CreateDbParameter("@depttype", deptType));
                    sqlparam.Add(DbParameters.CreateDbParameter("@encode", deptCode + '%'));
                }

            }
            //sql += @" group by b.DEPTNAME,b.REALNAME,a.usertype,b.deptcode,b.depttype,b.DEPTSORT,t.ISREFER ORDER BY b.DEPTSORT,b.DEPTCODE
            //        )
            //        select fullname,count(REALNAME) as person,usertype,depttype from table01 where 1=1";

            builder.AppendFormat(@" group by b.DEPTNAME,b.REALNAME,b.ISEPIBOLY,b.deptcode,b.depttype,b.DEPTSORT,t.ISREFER,t.USERID ORDER BY b.DEPTSORT,b.DEPTCODE
                    )
                    select fullname,count(REALNAME) as person,depttype from table01 where 1=1");

            if (!curuser.RoleName.IsEmpty())
            {
                string roleName = curuser.RoleName.ToString();
                var bmname = GetBmname(curuser.UserId);

                string accounts = this.BaseRepository().FindTable("SELECT ITEMVALUE FROM BASE_DATAITEMDETAIL WHERE ITEMNAME='SpecialAccount'").Rows[0]["ITEMVALUE"].ToString();
                
                int IsAppointAccount = 0;
                if (!string.IsNullOrEmpty(accounts))
                {
                    List<string> accountArray = accounts.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    Operator user = OperatorProvider.Provider.Current();
                    if (accountArray.Contains(user.Account.ToLower()))
                        IsAppointAccount = 1;
                }


                if (IsAppointAccount != 1)
                {
                    if (roleName.Contains("承包商级用户"))
                    {
                        //sql += string.Format(" and ISREFER is NULL");
                        //sql += string.Format(@" and FULLNAME = '{0}'", bmname);

                        builder.AppendFormat(@" and (ISREFER is NULL or userid = '{0}')", curuser.UserId);
                        builder.AppendFormat(" and FULLNAME = @FULLNAME");
                        sqlparam.Add(DbParameters.CreateDbParameter("@FULLNAME", bmname));
                    }
                    else if (roleName.Contains("厂级部门用户") || roleName.Contains("公司领导") || roleName.Contains("公司管理员") || roleName.Contains("公司级用户") || roleName.Contains("超级管理员") || curuser.UserId.Contains("1521c21a-62c9-4aa1-9093-a8bda503ea89"))
                    {
                        //此级别的用户可查看所有数据
                    }
                    else
                    {
                        //sql += string.Format(" and ISREFER is NULL");
                        //sql += string.Format(@" and FULLNAME = '{0}' or DEPTTYPE != '内部'", bmname);

                        builder.AppendFormat(" and (ISREFER is NULL or userid = '{0}')", curuser.UserId);
                        builder.AppendFormat(" and (FULLNAME = @FULLNAME or DEPTTYPE != '内部')");
                        sqlparam.Add(DbParameters.CreateDbParameter("@FULLNAME", bmname));
                    }
                }
            }

            //sql += @" group by fullname,usertype,depttype,DEPTCODE,DEPTSORT ORDER BY USERTYPE,DEPTTYPE,DEPTSORT,DEPTCODE";

            builder.AppendFormat(@" group by fullname,depttype,DEPTCODE,DEPTSORT ORDER BY DEPTTYPE,DEPTSORT,DEPTCODE");

            DataTable result = this.BaseRepository().FindTable(builder.ToString(), sqlparam.ToArray());
            //return this.BaseRepository().FindTable(sql);
            return result;
        }

        /// <summary>
        /// 根据部门名称加载人员数据
        /// </summary>
        /// <param name="deptName"></param>
        /// <param name="personName"></param>
        /// <returns></returns>
        public DataTable GetTableByDeptname(Pagination pagination, string deptName, string personName)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            if (!deptName.IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and (
                          case when (b.ISEPIBOLY = '否' and b.DEPTTYPE is null and length(b.deptcode)>20) then (select fullname from base_department d where d.deptcode = substr(b.deptcode,1,20)) 
										else b.DEPTNAME end
                                            ) = '{0}'", deptName);
            }
            if (!personName.IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and b.REALNAME ='{0}'", personName);
            }

            if (!curuser.RoleName.IsEmpty())
            {
                string roleName = curuser.RoleName.ToString();
                var bmname = GetBmname(curuser.UserId);

                string accounts = this.BaseRepository().FindTable("SELECT ITEMVALUE FROM BASE_DATAITEMDETAIL WHERE ITEMNAME='SpecialAccount'").Rows[0]["ITEMVALUE"].ToString();

                int IsAppointAccount = 0;
                if (!string.IsNullOrEmpty(accounts))
                {
                    List<string> accountArray = accounts.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    Operator user = OperatorProvider.Provider.Current();
                    if (accountArray.Contains(user.Account.ToLower()))
                        IsAppointAccount = 1;
                }

                if (IsAppointAccount != 1)
                {
                    if (roleName.Contains("承包商级用户"))
                    {
                        pagination.conditionJson += string.Format(" and (t.ISREFER is NULL or t.userid = '{0}')", curuser.UserId);
                    }
                    else if (roleName.Contains("厂级部门用户") || roleName.Contains("公司领导") || roleName.Contains("公司管理员") || roleName.Contains("公司级用户") || roleName.Contains("超级管理员") || curuser.UserId.Contains("1521c21a-62c9-4aa1-9093-a8bda503ea89"))
                    {
                        //此级别的用户可查看所有数据
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and (t.ISREFER is NULL or t.userid = '{0}')", curuser.UserId);
                    }
                }
            }


            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取所有人员门禁进出统计
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTableUserRole(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            string sqlint_out = "";
            var bmname = GetBmname(curuser.UserId);
            //判断时间
            if (!queryParam["sTime"].IsEmpty() && !queryParam["eTime"].IsEmpty())
            {
                string sTime = queryParam["sTime"].ToString() + " 00:00:00";
                string eTime = queryParam["eTime"].ToString() + " 23:59:59";

                pagination.p_fields = string.Format(@"v_info.NATURE,v_info.DEPTNAME,v_info.DEPARTMENTCODE,v_info.ORGANIZEID,row_number() over(partition by case when v_info.nature = '班组' then v_info.parentname else v_info.DEPTNAME end order by v_info.REALNAME asc) as shu,
                    case when (length(v_info.deptcode)>20) then (select d.SORTCODE from base_department d where d.deptcode = substr(v_info.deptcode,1,20)) else v_info.DEPTSORT end as DEPTSORTss,case when v_info.nature = '班组' then v_info.parentname else v_info.DEPTNAME end as bmname
                                    ,v_info.realname,v_info.dutyname,(select count(b.userid) from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) b where b.userid = v_info.userid and b.inout = 0 and 
                                    b.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and b.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss')) as intnum,(select count(c.userid) from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) c where c.userid = v_info.userid and c.inout = 1
                                    and c.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and c.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss')) as outnum,d.depttype,'' as bminnum,'' as bmoutnum", sTime, eTime);
            }


            //新增部门Code查询条件
            if (!queryParam["deptCode"].IsEmpty())
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string deptCode = queryParam["deptCode"].ToString();
                string deptType = "";
                if (deptCode.StartsWith("cx100_"))
                {
                    deptCode = deptCode.Replace("cx100_", "");
                    deptType = "长协";
                }
                if (deptCode.StartsWith("ls100_"))
                {
                    deptCode = deptCode.Replace("ls100_", "");
                    deptType = "临时";
                }
                if (queryParam["isOrg"].ToString() == "Organize")
                {
                    pagination.conditionJson += string.Format(" and v_info.organizecode='{0}'", deptCode);
                    sqlint_out += string.Format(" and b.organizecode='{0}'", deptCode);
                }
                else if (queryParam["isOrg"].ToString() == "Department" || user.IsSystem)
                {
                    pagination.conditionJson += string.Format(" and (v_info.departmentcode like '{0}%' or v_info.nickname  in (select departmentid from base_department where encode like '{0}%'))", deptCode);
                    sqlint_out += string.Format(" and (b.departmentcode like '{0}%' or b.nickname  in (select departmentid from base_department where encode like '{0}%'))", deptCode);
                }
                else
                {
                    pagination.conditionJson += string.Format(" and (v_info.departmentcode='{0}' or v_info.nickname in (select departmentid from base_department where encode='{0}'))", deptCode);
                    sqlint_out += string.Format(" and (b.departmentcode='{0}' or b.nickname in (select departmentid from base_department where encode='{0}'))", deptCode);
                }
                if (!string.IsNullOrWhiteSpace(deptType))
                {
                    pagination.conditionJson += string.Format(" and v_info.departmentid in(select departmentid from base_department d where d.depttype='{0}' and d.encode like '{1}%')", deptType, deptCode);
                    sqlint_out += string.Format(" and b.departmentid in (select departmentid from base_department d where d.depttype='{0}' and d.encode like '{1}%')", deptType, deptCode);
                }

            }
            //判断是内部单位还是外包单位
            if (!queryParam["state"].IsEmpty())
            {
                string state = queryParam["state"].ToString();
                string RoleName = curuser.RoleName;
                string sTime = queryParam["sTime"].ToString() + " 00:00:00";
                string eTime = queryParam["eTime"].ToString() + " 23:59:59";

                if (state == "0")
                {
                    pagination.conditionJson += string.Format(" and v_info.nature != '承包商'");

                    if (RoleName.Contains("厂级部门用户") || RoleName.Contains("公司领导") || RoleName.Contains("公司管理员") || RoleName.Contains("公司级用户") || RoleName.Contains("超级管理员") || curuser.UserId.Contains("1521c21a-62c9-4aa1-9093-a8bda503ea89"))
                    {
                        //此级别的用户可查看所有数据
                        pagination.p_fields += string.Format(@",(select COUNT(a.USERID) as num from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) a left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 0 and 
                            a.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and a.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss') and (case when b.depttype is null then '内部' else b.depttype end) = '内部' {2}) as allintnum,
                            (select COUNT(a.USERID) as num from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) a left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 1 and 
                            a.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and a.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss') and (case when b.depttype is null then '内部' else b.depttype end) = '内部' {2}) as alloutnum", sTime, eTime, sqlint_out);
                    }
                    else
                    {
                        pagination.p_fields += string.Format(@",(select COUNT(a.USERID) as num from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) a left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 0 and 
                            a.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and a.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss') and (case when b.depttype is null then '内部' else b.depttype end) = '内部' 
                            and (case when (b.ISEPIBOLY = '否' and b.DEPTTYPE is null and length(b.deptcode)>20) then (select fullname from base_department d where d.deptcode = substr(b.deptcode,1,20)) else b.DEPTNAME end) = '{2}' {3}) as allintnum,
                            (select COUNT(a.USERID) as num from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) a left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 1 and 
                            a.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and a.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss') and (case when b.depttype is null then '内部' else b.depttype end) = '内部'
                            and (case when (b.ISEPIBOLY = '否' and b.DEPTTYPE is null and length(b.deptcode)>20) then (select fullname from base_department d where d.deptcode = substr(b.deptcode,1,20)) else b.DEPTNAME end) = '{2}' {3}) as alloutnum", sTime, eTime, bmname, sqlint_out);
                    }
                }
                else
                {
                    pagination.conditionJson += string.Format(" and v_info.nature = '承包商'");
                    pagination.sidx = string.Format("v_info.DEPTTYPE,v_info.DEPTSORT,v_info.DEPTNAME,v_info.DEPTCODE,v_info.USERID");
                    pagination.sord = string.Format("desc");
                    
                    if (RoleName.Contains("厂级部门用户") || RoleName.Contains("公司领导") || RoleName.Contains("公司管理员") || RoleName.Contains("公司级用户") || RoleName.Contains("超级管理员") || curuser.UserId.Contains("1521c21a-62c9-4aa1-9093-a8bda503ea89"))
                    {
                        //此级别的用户可查看所有数据
                        pagination.p_fields += string.Format(@",(select COUNT(a.USERID) as num from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) a   left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 0 and 
                            a.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and a.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss') and (case when b.depttype is null then '内部' else b.depttype end) != '内部' {2}) as allintnum,
                            (select COUNT(a.USERID) as num from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) a left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 1 and 
                            a.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and a.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss') and (case when b.depttype is null then '内部' else b.depttype end) != '内部'{2}) as alloutnum", sTime, eTime, sqlint_out);
                    }
                    else
                    {
                        
                        pagination.p_fields += string.Format(@",(select COUNT(a.USERID) as num from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) a left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 0 and 
                            a.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and a.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss') and (case when b.depttype is null then '内部' else b.depttype end) != '内部' 
                            and (case when (b.ISEPIBOLY = '否' and b.DEPTTYPE is null and length(b.deptcode)>20) then (select fullname from base_department d where d.deptcode = substr(b.deptcode,1,20)) else b.DEPTNAME end) = '{2}' {3}) as allintnum,
                            (select COUNT(a.USERID) as num from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) a left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 1 and 
                            a.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and a.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss') and (case when b.depttype is null then '内部' else b.depttype end) != '内部'
                            and (case when (b.ISEPIBOLY = '否' and b.DEPTTYPE is null and length(b.deptcode)>20) then (select fullname from base_department d where d.deptcode = substr(b.deptcode,1,20)) else b.DEPTNAME end) = '{2}' {3}) as alloutnum", sTime, eTime, bmname, sqlint_out);
                    }
                }
            }
            //判断单位类型
            if (!queryParam["deptType"].IsEmpty())
            {
                string deptType = queryParam["deptType"].ToString();
                string state = queryParam["state"].ToString();
                if (state == "1")
                {
                    pagination.conditionJson += string.Format(" and d.depttype = '{0}'", deptType);
                }
            }
            //判断数据类型
            if (!queryParam["dataType"].IsEmpty())
            {
                string sTime = queryParam["sTime"].ToString() + " 00:00:00";
                string eTime = queryParam["eTime"].ToString() + " 23:59:59";
                string resultType = queryParam["dataType"].ToString();
                if (resultType == "0")
                {
                    pagination.conditionJson += string.Format(@" and ((select count(b.id) from bis_hikinoutlog b where b.userid = v_info.userid and b.inout = 0 and b.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and b.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss'))>0 or
                                                                    (select count(b.id) from bis_hikinoutlog b where b.userid = v_info.userid and b.inout = 1 and b.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and b.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss'))>0)", sTime, eTime);
                }
                else
                {
                    pagination.conditionJson += string.Format(@" and ((select count(b.id) from bis_hikinoutlog b where b.userid = v_info.userid and b.inout = 0 and b.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and b.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss'))=0 and
                                                                    (select count(b.id) from bis_hikinoutlog b where b.userid = v_info.userid and b.inout = 1 and b.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and b.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss'))=0)", sTime, eTime);
                }
            }
            //用户姓名
            if (!queryParam["Name"].IsEmpty())
            {
                string PersonName = queryParam["Name"].ToString();
                pagination.conditionJson += string.Format(" and v_info.REALNAME like '%{0}%'", PersonName);
            }
            
            //判断当前登陆用户是什么级别
            if (!curuser.RoleName.IsEmpty())
            {
                string RoleName = curuser.RoleName.ToString();

                string accounts = this.BaseRepository().FindTable("SELECT ITEMVALUE FROM BASE_DATAITEMDETAIL WHERE ITEMNAME='SpecialAccount'").Rows[0]["ITEMVALUE"].ToString();

                int IsAppointAccount = 0;
                if (!string.IsNullOrEmpty(accounts))
                {
                    List<string> accountArray = accounts.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    Operator user = OperatorProvider.Provider.Current();
                    if (accountArray.Contains(user.Account.ToLower()))
                        IsAppointAccount = 1;
                }
                if (IsAppointAccount != 1)
                {
                    if (RoleName.Contains("承包商级用户"))
                    {
                        //承包商级用户只可查看本单位门禁数据
                        //var bmname = GetBmname(curuser.UserId);
                        pagination.conditionJson += string.Format(" and (t.ISREFER is NULL or t.userid = '{0}')", curuser.UserId);
                        pagination.conditionJson += string.Format(" and v_info.ROLENAME like '%{0}%' and v_info.DEPTNAME = '{1}'", RoleName, bmname);
                    }
                    else if (RoleName.Contains("厂级部门用户") || RoleName.Contains("公司领导") || RoleName.Contains("公司管理员") || RoleName.Contains("公司级用户") || RoleName.Contains("超级管理员") || curuser.UserId.Contains("1521c21a-62c9-4aa1-9093-a8bda503ea89"))
                    {
                        //此级别的用户可查看所有数据
                    }
                    else
                    {
                        //var bmname = GetBmname(curuser.UserId);
                        pagination.conditionJson += string.Format(" and (t.ISREFER is NULL or t.userid = '{0}')", curuser.UserId);
                        pagination.conditionJson += string.Format(" and (v_info.ROLENAME like '%承包商级用户%' or v_info.DEPTNAME = '{0}' or v_info.PARENTNAME = '{0}')", bmname);
                    }
                }
                
            }
            int num = pagination.p_fields.Length;
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 根据用户ID获取部门名称
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetBmname(string userId)
        {
            string sql = string.Format(@"select case when a.nature = '班组' then a.parentname else a.DEPTNAME end as bmname 
                                                from v_userinfo a where a.USERID = '{0}'", userId);

            var result = this.BaseRepository().FindTable(sql);

            return result.Rows[0]["bmname"].ToString();
        }

        /// <summary>
        /// 获取人员门禁进出详细
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTableByUserID(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //根据用户ID查看门禁进出详细记录
            if (!queryParam["userID"].IsEmpty())
            {
                string userID = queryParam["userID"].ToString();

                pagination.conditionJson += string.Format(" and userid = '{0}'", userID);
            }
            if (!queryParam["sTime"].IsEmpty())
            {
                string sTime = queryParam["sTime"].ToString() + " 00:00:00";

                pagination.conditionJson += string.Format(" and CREATEDATE+0 >= to_date('{0}','yyyy-MM-dd hh24:mi:ss')", sTime);
            }
            if (!queryParam["eTime"].IsEmpty())
            {
                string eTime = queryParam["eTime"].ToString() + " 23:59:59";

                pagination.conditionJson += string.Format(" and CREATEDATE+0 <= to_date('{0}','yyyy-MM-dd hh24:mi:ss')", eTime);
            }
            //查看进出的状态
            if (!queryParam["inOut"].IsEmpty())
            {
                string inOut = queryParam["inOut"].ToString();

                pagination.conditionJson += string.Format(" and inout = '{0}'", inOut);
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取人员设置表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="ModuleType"></param>
        /// <returns></returns>
        public DataTable GetPersonSet(Pagination pagination, string ModuleType)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (!string.IsNullOrEmpty(ModuleType))
            {
                pagination.conditionJson += string.Format(" and a.MODULETYPE = '{0}'", ModuleType);
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取车辆测速数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable Get_BIS_CARVIOLATION(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["StartTime"].IsEmpty())
            {
                string sTime = queryParam["StartTime"].ToString();
                pagination.conditionJson += string.Format(@" and CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss')", sTime);
            }
            if (!queryParam["EndTime"].IsEmpty())
            {
                string eTime = queryParam["EndTime"].ToString();
                pagination.conditionJson += string.Format(@" and CREATEDATE <= to_date('{0}','yyyy-MM-dd hh24:mi:ss')", eTime);
            }
            if (!queryParam["txt_Keyword"].IsEmpty())
            {
                string type = queryParam["type"].ToString();
                string values = queryParam["txt_Keyword"].ToString();
                if (type == "0")
                {
                    pagination.conditionJson += string.Format(@" and CARDNO = '{0}'", values);
                }
                else if (type == "1")
                {
                    pagination.conditionJson += string.Format(@" and ADDRESS = '{0}'", values);
                }
                else {
                    pagination.conditionJson += string.Format(@" and DIRVER = '{0}'", values);
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 人员通道提交
        /// </summary>
        /// <param name="insert"></param>
        /// <param name="update"></param>
        public void UserAisleSave(HikinoutlogEntity insert, HikinoutlogEntity update)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (update != null)
                {
                    res.Update<HikinoutlogEntity>(update);
                }

                res.Insert<HikinoutlogEntity>(insert);
                res.Commit();
            }
            catch (Exception e)
            {
                res.Rollback();
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
        public void SaveForm(string keyValue, HikinoutlogEntity entity)
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

        /// <summary>
        /// 根据主键ID修改离场状态
        /// </summary>
        /// <param name="keyValue"></param>
        public void UpdateByID(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                string sql = string.Format(@"update bis_hikinoutlog set INOUT = 1,OUTTIME = TO_DATE(TO_CHAR(SYSDATE, 'yyyy-MM-dd hh24:mi:ss'), 'YYYY-MM-DD HH24:MI:SS'),DEVICENAME = '--' where ID = (
                    select ID from(select row_number() over(ORDER BY CREATEDATE desc) as num, CREATEDATE, ID from bis_hikinoutlog where USERID = '{0}') where num = 1
                    )", keyValue);
                this.BaseRepository().ExecuteBySql(sql);
            }
        }

        /// <summary>
        /// 根据人员设置ID删除数据
        /// </summary>
        public void DeletePersonSet(string keyValue)
        {
            var res = DbFactory.Base().BeginTrans();

            try
            {
                string[] list = keyValue.Split(',');
                res.Delete<PersonSetEntity>(it => list.Contains(it.PersonSetId));
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }


        /// <summary>
        /// 批量设置新增人员不可查询门禁
        /// </summary>
        public void SavePersonSet(string json, string ModuleType)
        {
            var res = DbFactory.Base().BeginTrans();

            try
            {
                IList<PersonSetEntity> entityList = new List<PersonSetEntity>();
                string[] list = json.Split(',');

                foreach (var item in list)
                {
                    PersonSetEntity entity = new PersonSetEntity();

                    var result = SelSavePersonSet(item.ToString(), ModuleType);
                    if (result == false)
                    {
                        entity.Create();
                        entity.UserId = item.ToString();
                        entity.ModuleType = ModuleType;
                        entity.Isrefer = 0;
                        entityList.Add(entity);
                    }
                }
                res.Insert<PersonSetEntity>(entityList);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }

        public List<int[]> GetPersonData()
        {
            var from = DateTime.Today;
            var to = from.AddDays(1);

            //电厂生产区
            var data11 = this.BaseRepository().IQueryable().Where(x => x.CreateDate >= from && x.CreateDate < to && x.AreaName == "一号岗" && x.DeviceType == 0 && x.IsOut == 0 && x.UserType == 0).Count();
            //电厂基建区
            var data12 = this.BaseRepository().IQueryable().Where(x => x.CreateDate >= from && x.CreateDate < to && x.AreaName == "三号岗" && x.DeviceType == 0 && x.IsOut == 0 && x.UserType == 0).Count();
            //电厂码头港
            var data13 = this.BaseRepository().IQueryable().Where(x => x.CreateDate >= from && x.CreateDate < to && x.AreaName == "码头岗" && x.DeviceType == 0 && x.IsOut == 0 && x.UserType == 0).Count();
            //合计
            //data11 = 140;
            //data12 = 26;
            var data14 = data11 + data12 + data13;
            //外委生产区
            var data21 = this.BaseRepository().IQueryable().Where(x => x.CreateDate >= from && x.CreateDate < to && x.AreaName == "一号岗" && x.DeviceType == 0 && x.IsOut == 0 && x.UserType == 1).Count();
            //外委基建区
            var data22 = this.BaseRepository().IQueryable().Where(x => x.CreateDate >= from && x.CreateDate < to && x.AreaName == "三号岗" && x.DeviceType == 0 && x.IsOut == 0 && x.UserType == 1).Count();
            //外委码头港
            var data23 = this.BaseRepository().IQueryable().Where(x => x.CreateDate >= from && x.CreateDate < to && x.AreaName == "码头岗" && x.DeviceType == 0 && x.IsOut == 0 && x.UserType == 1).Count();
            //合计
            //data21 = 324;
            //data22 = 55;
            var data24 = data21 + data22 + data23;
            //临时生产区
            var data31 = this.BaseRepository().IQueryable().Where(x => x.CreateDate >= from && x.CreateDate < to && x.AreaName == "一号岗" && x.DeviceType == 0 && x.IsOut == 0 && x.UserType == 2).Count();
            //临时基建区
            var data32 = this.BaseRepository().IQueryable().Where(x => x.CreateDate >= from && x.CreateDate < to && x.AreaName == "三号岗" && x.DeviceType == 0 && x.IsOut == 0 && x.UserType == 2).Count();
            //临时码头港
            var data33 = this.BaseRepository().IQueryable().Where(x => x.CreateDate >= from && x.CreateDate < to && x.AreaName == "码头岗" && x.DeviceType == 0 && x.IsOut == 0 && x.UserType == 2).Count();
            //合计
            var data34 = data31 + data32 + data33;
            return new List<int[]> {
                new int[] { data11, data12, data13, data14 },
                new int[] { data21, data22, data23, data24 },
                new int[] { data31, data32, data33, data34 },
                new int[] { data11 + data21 + data31, data12 + data22 + data32, data13 + data23 + data33, data14 + data24 + data34 },
            };
        }

        public int[] GetAreaData()
        {
            var from = DateTime.Today;
            var to = from.AddDays(1);

            var result = new List<int>();
            var db = DbFactory.Base();
            var categories = (from q1 in db.IQueryable<DataItemDetailEntity>()
                              join q2 in db.IQueryable<DataItemEntity>() on q1.ItemId equals q2.ItemId
                              where q2.ItemName == "区域门禁点映射"
                              orderby q1.SortCode ascending
                              select q1).ToList();

            foreach (var item in categories)
            {
                var areas = item.ItemValue.Split(',').ToArray();
                var current = from q in db.IQueryable<DistrictEntity>()
                              where areas.Any(x => x == q.DistrictID)
                              select q;
                var subquery = from q1 in current
                               join q2 in db.IQueryable<DistrictEntity>() on q1.DistrictID equals q2.ParentID
                               select q2;
                while (subquery.Count() > 0)
                {
                    current = current.Concat(subquery);
                    subquery = from q1 in subquery
                               join q2 in db.IQueryable<DistrictEntity>() on q1.DistrictID equals q2.ParentID
                               select q2;
                }

                var totalQuery = from q1 in db.IQueryable<HikinoutlogEntity>()
                                 join q2 in db.IQueryable<HikaccessEntity>() on q1.DeviceHikID equals q2.HikId
                                 join q3 in current.Distinct() on q2.AreaId equals q3.DistrictID
                                 where q1.CreateDate >= @from && q1.CreateDate < to && q1.DeviceType == 2 && q1.IsOut == 0
                                 select q1;
                var total = totalQuery.Count();
                result.Add(total);
            }

            return result.ToArray();
        }

        /// <summary>
        /// 获取当天的人员进出数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetTodayCarPeopleCount()
        {
            //!!!京泰电厂 1号岗、2号岗为进厂与离厂通道，其余门岗进出不算入统计之内
            DateTime startTime = DateTime.Now.Date;
            DateTime endTime = startTime.AddDays(1);
            string sql = string.Format(@" 
	 SELECT * FROM (SELECT COUNT(*) AS PEOPLEIN FROM BIS_HIKINOUTLOG WHERE INOUT=0 AND CREATEDATE>TO_DATE('{0}', 'yyyy-MM-dd') AND CREATEDATE<TO_DATE('{1}', 'yyyy-MM-dd') and DEVICEHIKID in (select ITEMVALUE from BASE_DATAITEMDETAIL where itemid in ('a665b9a3-f24a-4876-ae0e-8830336f151c','8ee4c49e-8b4e-499e-96ab-6a1db80ab36c'))) ,
  (SELECT COUNT(*)  AS PEOPLEOUT FROM BIS_HIKINOUTLOG WHERE INOUT=1 AND CREATEDATE>TO_DATE('{0}', 'yyyy-MM-dd') AND CREATEDATE<TO_DATE('{1}', 'yyyy-MM-dd') and DEVICEHIKID in (select ITEMVALUE from BASE_DATAITEMDETAIL where itemid in ('a665b9a3-f24a-4876-ae0e-8830336f151c','8ee4c49e-8b4e-499e-96ab-6a1db80ab36c'))),
	(SELECT  COUNT(*)  AS CARIN FROM BIS_CARINLOG WHERE STATUS=0 AND CREATEDATE>TO_DATE('{0}', 'yyyy-MM-dd') AND CREATEDATE<TO_DATE('{1}', 'yyyy-MM-dd') and (ADDRESS like '%2#岗亭%' OR ADDRESS LIKE '%1#岗亭%')),
	(SELECT  COUNT(*)  AS CAROUT FROM BIS_CARINLOG WHERE STATUS=1  AND CREATEDATE>TO_DATE('{0}', 'yyyy-MM-dd') AND CREATEDATE<TO_DATE('{1}', 'yyyy-MM-dd') and (ADDRESS like '%2#岗亭%' OR ADDRESS LIKE '%1#岗亭%'))", startTime.ToString("yyyy-MM-dd"), endTime.ToString("yyyy-MM-dd"));
            DataTable dt = this.BaseRepository().FindTable(sql);
            Dictionary<string, int> dic = new Dictionary<string, int>();
            if (dt.Rows != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                var peopleIn = Convert.ToInt32(dr["PEOPLEIN"]);
                var peopleOut = Convert.ToInt32(dr["PEOPLEOUT"]);
                var carIn = Convert.ToInt32(dr["CARIN"]);
                var carOut = Convert.ToInt32(dr["CAROUT"]);
                dic.Add("PeopleIn", peopleIn);
                dic.Add("PeopleOut", peopleOut);
                dic.Add("CarIn", carIn);
                dic.Add("CarOut", carOut);
            }
            return dic;
        }

        /// <summary>
        /// 获取最新的车辆人员进出数据，取前五条
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetCarPeopleTopData()
        {
            var db = new RepositoryFactory().BaseRepository();
            var peopleList = db.IQueryable<HikinoutlogEntity>().OrderByDescending(x => x.CreateDate).Select(x => new
            {
                x.UserName,
                x.CreateDate,
                InOut = x.InOut == 1 ? "离开" : "进入",
                x.AreaName
            }).Take(5).ToList();

            var carList = db.IQueryable<CarinlogEntity>().OrderByDescending(x => x.CreateDate).Select(x => new
            {
                x.DriverName,
                x.CreateDate,
                InOut = x.Status == 1 ? "离开" : "进入",
                x.CarNo,
                x.Address
            }).Take(5).ToList();
            Dictionary<string, object> dic = new Dictionary<string, object> { { "People", peopleList }, { "Car", carList } };
            return dic;

        }

        #endregion
    }
}
