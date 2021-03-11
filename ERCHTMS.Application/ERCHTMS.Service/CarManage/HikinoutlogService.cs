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
    /// �� �����豸��¼��Ա������־
    /// </summary>
    public class HikinoutlogService : RepositoryFactory<HikinoutlogEntity>, HikinoutlogIService
    {
        #region ��ȡ����
        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //����
            if (!queryParam["Name"].IsEmpty())
            {
                string Name = queryParam["Name"].ToString();

                pagination.conditionJson += string.Format(" and USERNAME like '%{0}%'", Name);

            }
            //��������
            if (!queryParam["DeptName"].IsEmpty())
            {
                string DeptName = queryParam["DeptName"].ToString();

                pagination.conditionJson += string.Format(" and DeptName like '%{0}%'", DeptName);

            }
            //�豸����
            if (!queryParam["DeviceName"].IsEmpty())
            {
                string DeviceName = queryParam["DeviceName"].ToString();

                pagination.conditionJson += string.Format(" and DeviceName like '%{0}%'", DeviceName);

            }
            //��������
            if (!queryParam["OutType"].IsEmpty())
            {
                int OutType = Convert.ToInt32(queryParam["OutType"]);

                pagination.conditionJson += string.Format(" and OutType ={0}", OutType);

            }
            //�豸�����Ÿ�
            if (!queryParam["AreaName"].IsEmpty())
            {
                string AreaName = queryParam["AreaName"].ToString();
                pagination.conditionJson += string.Format(" and AreaName like '%{0}%'", AreaName);
            }
            //�¼�����
            if (!queryParam["EventType"].IsEmpty())
            {
                string EventType = queryParam["EventType"].ToString();
                pagination.conditionJson += string.Format(" and EventType = '{0}'", EventType);
            }
            //��ʼʱ��
            if (!queryParam["StartTime"].IsEmpty())
            {
                string StartTime = queryParam["StartTime"].ToString();
                pagination.conditionJson += string.Format(" and CREATEDATE>=TO_DATE('{0}','yyyy-MM-dd hh24:mi:ss') ", StartTime);
            }
            //����ʱ��
            if (!queryParam["EndTime"].IsEmpty())
            {
                string EndTime = queryParam["EndTime"].ToString();
                pagination.conditionJson += string.Format(" and CREATEDATE<=TO_DATE('{0}','yyyy-MM-dd hh24:mi:ss')>", EndTime);
            }
            //�û�����
            if (!queryParam["UserType"].IsEmpty())
            {
                string UserType = queryParam["UserType"].ToString();
                pagination.conditionJson += string.Format(" and UserType = {0}", UserType);
            }

            //�Ƿ����
            if (!queryParam["Isout"].IsEmpty())
            {
                string Isout = queryParam["Isout"].ToString();
                pagination.conditionJson += string.Format(" and Isout = {0}", Isout);
            }

            //��������
            if (!queryParam["Inout"].IsEmpty())
            {
                string Inout = queryParam["Inout"].ToString();
                pagination.conditionJson += string.Format(" and Inout = {0}", Inout);
            }



            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ����ȫ������
        /// </summary>
        /// <returns></returns>
        public DataTable GetNums()
        {
            string sql = string.Format(
                @"select 0 type,nvl(count(id),0) as sums,AREANAME from BIS_HIKINOUTLOG where inout=0 and isout=0 and USERTYPE=0 and createdate>=TO_DATE('{0}', 'yyyy-mm-dd HH24:mi:ss')  GROUP BY AREANAME HAVING AREANAME='���Ÿ�'  OR  AREANAME='��ͷ��' 
                    union all
                  select 1 type,nvl(count(id),0) as sums,AREANAME from BIS_HIKINOUTLOG where inout=0 and isout=0 and USERTYPE=1 and createdate>=TO_DATE('{0}', 'yyyy-mm-dd HH24:mi:ss')  GROUP BY AREANAME HAVING AREANAME='���Ÿ�'  OR  AREANAME='��ͷ��' 
                    union all
                  select 2 type,nvl(count(id),0) as sums,AREANAME from BIS_HIKINOUTLOG where inout=0 and isout=0 and USERTYPE=2 and createdate>=TO_DATE('{0}', 'yyyy-mm-dd HH24:mi:ss')  GROUP BY AREANAME HAVING AREANAME='���Ÿ�'  OR  AREANAME='��ͷ��' 
                    union  
				  select USERTYPE as type,nvl(count(id),0) as sums,AREANAME from BIS_HIKINOUTLOG where inout=0 and isout=0 and createdate>=TO_DATE('{1}', 'yyyy-mm-dd HH24:mi:ss')  and AREANAME='һ�Ÿ�' and ( USERTYPE=0 OR USERTYPE=1 OR USERTYPE=2) 
				  group by USERTYPE,AREANAME ", DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"));
            return this.BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// ��ȡ��ǰ԰���ڳ���
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
        /// ��ȡ�������һ��ˢ����¼
        /// </summary>
        /// <returns></returns>
        public HikinoutlogEntity GetLastInoutLog()
        {
            string sql = string.Format(@"SELECT * FROM (SELECT CREATEDATE ,USERNAME,DEPTNAME,INOUT,ISOUT,AREANAME,DEVICENAME FROM BIS_HIKINOUTLOG WHERE TO_CHAR(CREATEDATE,'yyyy-mm-dd')='{0}' AND INSTR('��ͷ��,һ�Ÿ�,���Ÿ�',AREANAME)>0 ORDER BY CREATEDATE DESC) WHERE ROWNUM=1", DateTime.Now.ToString("yyyy-MM-dd"));
            Repository<HikinoutlogEntity> inlogdb = new Repository<HikinoutlogEntity>(DbFactory.Base());
            List<HikinoutlogEntity> list = inlogdb.FindList(sql).ToList();

            return list.OrderByDescending(x => x.CreateDate).FirstOrDefault();

        }



        /// <summary>
        /// �����û�ID��ѯ��Ա���ñ�
        /// </summary>
        /// <param name="id">�û�ID</param>
        /// <param name="type">����ģ�����</param>
        /// <returns></returns>
        public bool SelSavePersonSet(string id, string type)
        {
            string sql = string.Format(@"select * from hjb_personset where userid = '{0}' and moduletype = '{1}'", id, type);

            var result = this.BaseRepository().FindTable(sql);

            return result.Rows.Count > 0;
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HikinoutlogEntity> GetList(string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            StringBuilder sql = new StringBuilder("SELECT CREATEDATE ,USERNAME,DEPTNAME,INOUT,ISOUT,AREANAME,DEVICENAME FROM BIS_HIKINOUTLOG WHERE 1=1 ");
            //��ԱID
            if (!queryParam["UserId"].IsEmpty())
            {
                string userId = queryParam["UserId"].ToString();
                sql.AppendFormat(" AND USERID = '{0}'", userId);
            }
            //��ʼʱ��
            if (!queryParam["StartDate"].IsEmpty())
            {
                string StartDate = queryParam["StartDate"].ToString();
                sql.AppendFormat(" AND  TO_CHAR(CREATEDATE,'yyyy-MM-dd')>='{0}'", StartDate);
            }
            //����ʱ��
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
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HikinoutlogEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// �����û�id��ѯ���û�����δ������¼
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
        /// �����豸ID
        /// </summary>
        /// <param name="HikId"></param>
        /// <returns></returns>
        public HikinoutlogEntity DeviceGetLog(string HikId)
        {
            string sql = string.Format("select * from BIS_HIKINOUTLOG  where devicehikid = '{0}' and inout=0 and isout=0 order by createdate asc", HikId);
            return this.BaseRepository().FindList(sql).FirstOrDefault();
        }

        /// <summary>
        /// ��ȡ����һ����������
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
        /// ��ȡ���ڵ�Code��ȡ���������ӽڵ�
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
        /// �ݹ����
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
        /// ����hikid�豸��ID��ȡ��Ա������ǰ��������
        /// </summary>
        /// <param name="hikId">�豸��Id</param>
        /// <returns></returns>
        public System.Collections.IList GetTopFiveById(string hikId)
        {
            return this.BaseRepository().IQueryable(x => x.DeviceHikID.Equals(hikId)).OrderByDescending(x => x.CreateDate).Select(x => new
            {
                x.CreateDate,
                x.UserName,
                x.DeptName,
                InOut = x.InOut == 0 ? "����" : "����",
                x.ScreenShot
            }).Take(5).ToList();

        }

        /// <summary>
        /// ��ȡ�豸���ص�һ������
        /// </summary>
        /// <returns></returns>
        public HikinoutlogEntity GetFirsetData()
        {
            return this.BaseRepository().IQueryable(x => x.DeviceType == 2).OrderByDescending(x => x.CreateDate).FirstOrDefault();
        }

        /// <summary>
        /// �����Ž����豸��Ż�ȡ��ر��
        /// </summary>
        /// <param name="DoorIndexCode">�Ž����豸���</param>
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
        /// ���ڸ澯
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
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

            //����
            if (!queryParam["RealName"].IsEmpty())
            {
                string Name = queryParam["RealName"].ToString();
                sb.AppendFormat(" and v.REALNAME like @RealName");
                param.Add(DbParameters.CreateDbParameter("@RealName", '%' + Name + '%'));
                //pagination.conditionJson += string.Format(" and v.REALNAME like '%{0}%'", Name);
            }
            //��������
            if (!queryParam["DeptType"].IsEmpty())
            {
                string DeptType = queryParam["DeptType"].ToString();
                sb.AppendFormat(" and v.DEPTTYPE= @DeptType");
                param.Add(DbParameters.CreateDbParameter("@DeptType", DeptType));
                //pagination.conditionJson += string.Format(" and v.DEPTTYPE= '{0}'", DeptType);
            }
            //�ڲ����������λ
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
                    deptType = "��Э";
                }
                if (deptCode.StartsWith("ls100_"))
                {
                    deptCode = deptCode.Replace("ls100_", "");
                    deptType = "��ʱ";
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
            //�жϵ�ǰ��½�û���ʲô����
            if (!curuser.RoleName.IsEmpty())
            {
                string RoleName = curuser.RoleName.ToString();
                string accounts = this.BaseRepository().FindTable("SELECT ITEMVALUE FROM BASE_DATAITEMDETAIL WHERE ITEMNAME='SpecialAccount'").Rows[0]["ITEMVALUE"].ToString();

                int IsAppointAccount = 0;
                if (!string.IsNullOrEmpty(accounts))//�ж��ض�Ȩ��
                {
                    List<string> accountArray = accounts.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    Operator user = OperatorProvider.Provider.Current();
                    if (accountArray.Contains(user.Account.ToLower()))
                        IsAppointAccount = 1;
                }
                if (IsAppointAccount != 1)
                {

                    if (!curuser.RoleName.Contains("���������û�") && !curuser.RoleName.Contains("��˾���û�"))
                    {
                        StringBuilder sqluser = new StringBuilder(@"SELECT USERID FROM HJB_PERSONSET WHERE MODULETYPE=2 AND USERID=@PERSONSETUserId");
                        //string sqluser = string.Format("SELECT USERID FROM HJB_PERSONSET WHERE MODULETYPE=2 AND USERID='{0}' ", curuser.UserId);
                        var param2 = new List<DbParameter>();
                        param2.Add(DbParameters.CreateDbParameter("@PERSONSETUserId", curuser.UserId));
                        DataTable dtperson = this.BaseRepository().FindTable(sqluser.ToString(), param2.ToArray());
                        if (dtperson != null && dtperson.Rows.Count > 0)//��ǰ�û��Ƿ�Ϊ���ñ�����û�- �Լ����Լ�
                        {
                            //pagination.conditionJson += string.Format(" and   v.USERID not in (SELECT USERID FROM HJB_PERSONSET WHERE MODULETYPE = 2  AND USERID<>'{0}') ", curuser.UserId);
                            sb.AppendFormat(" and   v.USERID not in (SELECT USERID FROM HJB_PERSONSET WHERE MODULETYPE = 2  AND USERID<>@UserId) ");
                            param.Add(DbParameters.CreateDbParameter("@UserId", curuser.UserId));
                        }
                        else//�����ض��û�Ҳ����������Ա-�����ܿ���������Ա
                        {
                            //pagination.conditionJson += string.Format(" and   v.USERID not in (SELECT USERID FROM HJB_PERSONSET WHERE MODULETYPE = 2 ) ");
                            sb.AppendFormat(" and   v.USERID not in (SELECT USERID FROM HJB_PERSONSET WHERE MODULETYPE = 2 ) ");
                        }

                    }
                    if (RoleName.Contains("�а��̼��û�"))
                    {
                        //�а��̼��û�ֻ�ɲ鿴����λ�Ž�����
                        var bmname = GetBmname(curuser.UserId);

                        //pagination.conditionJson += string.Format(" and v.ROLENAME like '%{0}%' and v.DEPTNAME = '{1}'", RoleName, bmname);
                        sb.AppendFormat(" and v.ROLENAME like @RoleName and v.DEPTNAME = @bmname");
                        param.Add(DbParameters.CreateDbParameter("@RoleName", '%' + RoleName + '%'));
                        param.Add(DbParameters.CreateDbParameter("@bmname", bmname));
                    }
                    else if (RoleName.Contains("���������û�") || RoleName.Contains("��˾����Ա") || RoleName.Contains("��˾���û�") || RoleName.Contains("��������Ա"))
                    {
                        //�˼�����û��ɲ鿴��������
                    }
                    else
                    {
                        var bmname = GetBmname(curuser.UserId);

                        //pagination.conditionJson += string.Format(" and (v.ROLENAME like '%�а��̼��û�%' or v.DEPTNAME = '{0}' or v.PARENTNAME = '{0}')", bmname);
                        sb.AppendFormat(" and (v.ROLENAME like '%�а��̼��û�%' or v.DEPTNAME = @bmname or v.PARENTNAME = @bmname)");
                        param.Add(DbParameters.CreateDbParameter("@bmname", bmname));
                    }
                }
            }
            //pagination.sord = "desc";
            //pagination.sidx = "v.DEPTSORT asc, v.SORTCODE asc ,v.USERID";
            DateTime DKStartTime = Convert.ToDateTime(queryParam["StartTime"].ToString());//��ʼ����
            DateTime DKEndTime = Convert.ToDateTime(queryParam["EndTime"].ToString());//��������
            int DayCount = Convert.ToInt32(queryParam["DayCount"]);//Ҫ��ѯ������δ������


            int rows = pagination.rows;
            int page = pagination.page;

            pagination.page = 1;
            pagination.rows = 1000000000;

            DataTable result = this.BaseRepository().FindTable(sb.ToString(), param.ToArray(), pagination);
            //DataTable result = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            for (int i = 0; i < result.Rows.Count; i++)
            {
                DataRow item = result.Rows[i];
                string remark = item["remark"].ToString();//�����ڼ��ϣ����Ÿ�����
                string[] dateArry = new string[0];//���������鼯��
                if (!string.IsNullOrEmpty(remark))
                    dateArry = remark.Split(',');
                int daydk = 0;//δ�򿨴���
                string dkremark = "";//����ǰ�˵�����δ����Ϣ
                DateTime sdate = new DateTime();//����һ������δ�򿨵�ʱ��εĿ�ʼ����
                for (DateTime d = DKStartTime; d <= DKEndTime; d = d.AddDays(1))//ѭ����ʼ��������
                {
                    if (dateArry.FirstOrDefault(t => t.Contains(d.ToString("yyyy-MM-dd"))) == null)//�ж��Ƿ���ڸ����ڵĴ���Ϣ
                    {
                        daydk++;//�ۼ�����
                        if (sdate < DKStartTime)
                            sdate = d;//��ֵ��ʼ����
                        if (daydk == DayCount)
                        {
                            dkremark += string.Format(" {0} ��{1}����{2}��ȱ��;", sdate.ToString("yyyy-MM-dd"), d.ToString("yyyy-MM-dd"), DayCount);
                            daydk = 0;//��ʼ����������
                            sdate = new DateTime();//��ʼ����ֵ��ʼ����
                        }
                    }
                    else
                    {
                        daydk = 0;//��ʼ����������
                        sdate = new DateTime();//��ʼ����ֵ��ʼ����
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
            //    string remark = item["remark"].ToString();//�����ڼ��ϣ����Ÿ�����
            //    string[] dateArry = new string[0];//���������鼯��
            //    if (!string.IsNullOrEmpty(remark))
            //        dateArry = remark.Split(',');
            //    int daydk = 0;//δ�򿨴���
            //    string dkremark = "";//����ǰ�˵�����δ����Ϣ
            //    DateTime sdate = new DateTime();//����һ������δ�򿨵�ʱ��εĿ�ʼ����
            //    for (DateTime d = DKStartTime; d <= DKEndTime; d = d.AddDays(1))//ѭ����ʼ��������
            //    {
            //        if (dateArry.FirstOrDefault(t => t.Contains(d.ToString("yyyy-MM-dd"))) == null)//�ж��Ƿ���ڸ����ڵĴ���Ϣ
            //        {
            //            daydk++;//�ۼ�����
            //            if (sdate < DKStartTime)
            //                sdate = d;//��ֵ��ʼ����
            //            if (daydk == DayCount)
            //            {
            //                dkremark += string.Format(" {0} ��{1}����{2}��ȱ��;", sdate.ToString("yyyy-MM-dd"), d.ToString("yyyy-MM-dd"), DayCount);
            //                daydk = 0;//��ʼ����������
            //                sdate = new DateTime();//��ʼ����ֵ��ʼ����
            //            }
            //        }
            //        else
            //        {
            //            daydk = 0;//��ʼ����������
            //            sdate = new DateTime();//��ʼ����ֵ��ʼ����
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
        /// ����ȱ��ͳ��
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetAbsenteeismPageList(Pagination pagination, string queryJson)
        {
//            StringBuilder sb = new StringBuilder(@"SELECT u.USERID, 
//u.DEPARTMENTID,u.DEPARTMENTCODE, u.REALNAME,u.DUTYID,u.DUTYNAME,u.depttype,
//case when u.nature = '����' then u.parentname else u.DEPTNAME end FULLNAME,
//COUNT(u.USERID) OVER(partition by case when u.nature = '����' then u.parentname else u.DEPTNAME end ) AS personcount,
//  case when (length(u.deptcode)>20) then (select d.SORTCODE from base_department d where d.deptcode = substr(u.deptcode,1,20)) else u.DEPTSORT end as DEPTSORTss
//FROM V_USERINFO u 
//left  JOIN  BIS_HIKINOUTLOG bh on bh.USERID = u.USERID ");
            //var param = new List<DbParameter>();
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();

            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime DKStartTime = Convert.ToDateTime(queryParam["StartTime"].ToString());//��ʼ����
                DateTime DKEndTime = Convert.ToDateTime(queryParam["EndTime"].ToString());//��������

                pagination.p_tablename += string.Format(@"  AND  TO_CHAR(bh.CREATEDATE, 'yyyy-MM-dd')>='{0}' 
                AND  TO_CHAR(bh.CREATEDATE, 'yyyy-MM-dd')<='{1}'   ", DKStartTime.ToString("yyyy-MM-dd"), DKEndTime.ToString("yyyy-MM-dd"));

                //sb.AppendFormat(@" AND  TO_CHAR(bh.CREATEDATE, 'yyyy-MM-dd')>=@DKStartTime 
                //AND  TO_CHAR(bh.CREATEDATE, 'yyyy-MM-dd') <= @DKEndTime");
                //sb.AppendFormat(@" AND  TO_CHAR(bh.CREATEDATE, 'yyyy-MM-dd')>='{0}' 
                //AND  TO_CHAR(bh.CREATEDATE, 'yyyy-MM-dd')<='{1}'   ", DKStartTime.ToString("yyyy-MM-dd"), DKEndTime.ToString("yyyy-MM-dd"));
                //param.Add(DbParameters.CreateDbParameter("@DKStartTime", DKStartTime.ToString("yyyy-MM-dd")));
                //param.Add(DbParameters.CreateDbParameter("@DKEndTime", DKEndTime.ToString("yyyy-MM-dd")));

                //---------��ѯ����
               // sb.AppendFormat(@" where u.DEPARTMENTID not in ('0')  AND  TO_CHAR(bh.CREATEDATE, 'yyyy-MM-dd') is NULL  AND u.USERID not in (SELECT USERID FROM HJB_PERSONSET WHERE MODULETYPE = 3)");
                //����
                if (!queryParam["RealName"].IsEmpty())
                {
                    string Name = queryParam["RealName"].ToString();

                    pagination.conditionJson += string.Format(" and u.REALNAME like '%{0}%'", Name);
                    //sb.AppendFormat(@"  and u.REALNAME like @RealName");
                    //sb.AppendFormat(@"  and u.REALNAME like '%{0}%'", Name);
                    //param.Add(DbParameters.CreateDbParameter("@RealName", '%' + Name + '%'));
                }
                //��������
                if (!queryParam["DeptType"].IsEmpty())
                {
                    string DeptType = queryParam["DeptType"].ToString();

                    pagination.conditionJson += string.Format(" and u.DEPTTYPE= '{0}'", DeptType);
                    //sb.AppendFormat(" and u.DEPTTYPE= @DeptType");
                    //sb.AppendFormat(" and u.DEPTTYPE= '{0}'", DeptType);
                    //param.Add(DbParameters.CreateDbParameter("@DeptType", DeptType));
                }
                //�ڲ����������λ
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
                        deptType = "��Э";
                    }
                    if (deptCode.StartsWith("ls100_"))
                    {
                        deptCode = deptCode.Replace("ls100_", "");
                        deptType = "��ʱ";
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
                //�жϵ�ǰ��½�û���ʲô����
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
                        if (RoleName.Contains("�а��̼��û�"))
                        {
                            //�а��̼��û�ֻ�ɲ鿴����λ�Ž�����
                            var bmname = GetBmname(curuser.UserId);

                            pagination.conditionJson += string.Format(" and u.ROLENAME like '%{0}%' and u.DEPTNAME = '{1}'", RoleName, bmname);
                            //sb.AppendFormat(" and u.ROLENAME like @RoleName and u.DEPTNAME = @bmname");
                            //sb.AppendFormat(" and u.ROLENAME like '%{0}%' and u.DEPTNAME = '{1}'", RoleName, bmname);
                            //param.Add(DbParameters.CreateDbParameter("@RoleName", '%' + RoleName + '%'));
                            //param.Add(DbParameters.CreateDbParameter("@bmname", bmname));
                        }
                        else if (RoleName.Contains("���������û�") || RoleName.Contains("��˾����Ա") || RoleName.Contains("��˾���û�") || RoleName.Contains("��������Ա"))
                        {
                            //�˼�����û��ɲ鿴��������
                        }
                        else
                        {
                            var bmname = GetBmname(curuser.UserId);

                            pagination.conditionJson += string.Format(" and (u.ROLENAME like '%�а��̼��û�%' or u.DEPTNAME = '{0}' or u.PARENTNAME = '{0}')", bmname);
                            //sb.AppendFormat(" and (u.ROLENAME like '%�а��̼��û�%' or u.DEPTNAME = @bmname or u.PARENTNAME = @bmname)");
                            //sb.AppendFormat(" and (u.ROLENAME like '%�а��̼��û�%' or u.DEPTNAME = '{0}' or u.PARENTNAME = '{0}')", bmname);
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
        /// ��ȡ����ȱ��ͳ����Ա���ñ�����
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
        /// ������Ա����IDɾ������
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
        /// ��������������Ա���ɲ�ѯ�Ž�
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
        /// ��ȡ���ڸ澯��Ա���ñ�����
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
        /// ������Ա����IDɾ������
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
        /// ��������������Ա���ɲ�ѯ�Ž�
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
        /// ��ȡ�����ŵ��ڳ���Աͳ������
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
                                        case when (b.ISEPIBOLY = '��' and b.DEPTTYPE is null and length(b.deptcode)>20) then  
									(select d.SORTCODE from base_department d where d.deptcode = substr(b.deptcode,1,20)) else b.DEPTSORT end as DEPTSORT,
                                        b.REALNAME,b.ISEPIBOLY,case when b.depttype is null then '�ڲ�' else b.depttype end as depttype,
										case when (b.ISEPIBOLY = '��' and b.DEPTTYPE is null and length(b.deptcode)>20) then (select fullname from base_department d where d.deptcode = substr(b.deptcode,1,20)) 
										else b.DEPTNAME end as fullname
									  from bis_hikinoutlog a
                                      left join V_USERINFO b on a.userid = b.userid
                                      left join (select * from HJB_PERSONSET where MODULETYPE = 0) t on a.userid = t.userid
                                      where b.DEPTNAME is not null and a.INOUT = 0 and not exists(select 1 from bis_hikinoutlog d where d.userid = a.userid and d.CREATEDATE+0 > a.CREATEDATE+0)");
            //����
            if (!queryParam["Name"].IsEmpty())
            {
                string Name = queryParam["Name"].ToString();

                //sql += string.Format(" and USERNAME like '%{0}%'", Name);

                builder.AppendFormat(@" and b.REALNAME like @Name");
                sqlparam.Add(DbParameters.CreateDbParameter("@Name", '%' + Name + '%'));
            }
            //��������Code��ѯ����
            if (!queryParam["deptCode"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string deptCode = queryParam["deptCode"].ToString();
                string deptType = "";
                if (deptCode.StartsWith("cx100_"))
                {
                    deptCode = deptCode.Replace("cx100_", "");
                    deptType = "��Э";
                }
                if (deptCode.StartsWith("ls100_"))
                {
                    deptCode = deptCode.Replace("ls100_", "");
                    deptType = "��ʱ";
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
                    if (roleName.Contains("�а��̼��û�"))
                    {
                        //sql += string.Format(" and ISREFER is NULL");
                        //sql += string.Format(@" and FULLNAME = '{0}'", bmname);

                        builder.AppendFormat(@" and (ISREFER is NULL or userid = '{0}')", curuser.UserId);
                        builder.AppendFormat(" and FULLNAME = @FULLNAME");
                        sqlparam.Add(DbParameters.CreateDbParameter("@FULLNAME", bmname));
                    }
                    else if (roleName.Contains("���������û�") || roleName.Contains("��˾�쵼") || roleName.Contains("��˾����Ա") || roleName.Contains("��˾���û�") || roleName.Contains("��������Ա") || curuser.UserId.Contains("1521c21a-62c9-4aa1-9093-a8bda503ea89"))
                    {
                        //�˼�����û��ɲ鿴��������
                    }
                    else
                    {
                        //sql += string.Format(" and ISREFER is NULL");
                        //sql += string.Format(@" and FULLNAME = '{0}' or DEPTTYPE != '�ڲ�'", bmname);

                        builder.AppendFormat(" and (ISREFER is NULL or userid = '{0}')", curuser.UserId);
                        builder.AppendFormat(" and (FULLNAME = @FULLNAME or DEPTTYPE != '�ڲ�')");
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
        /// ���ݲ������Ƽ�����Ա����
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
                          case when (b.ISEPIBOLY = '��' and b.DEPTTYPE is null and length(b.deptcode)>20) then (select fullname from base_department d where d.deptcode = substr(b.deptcode,1,20)) 
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
                    if (roleName.Contains("�а��̼��û�"))
                    {
                        pagination.conditionJson += string.Format(" and (t.ISREFER is NULL or t.userid = '{0}')", curuser.UserId);
                    }
                    else if (roleName.Contains("���������û�") || roleName.Contains("��˾�쵼") || roleName.Contains("��˾����Ա") || roleName.Contains("��˾���û�") || roleName.Contains("��������Ա") || curuser.UserId.Contains("1521c21a-62c9-4aa1-9093-a8bda503ea89"))
                    {
                        //�˼�����û��ɲ鿴��������
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
        /// ��ȡ������Ա�Ž�����ͳ��
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
            //�ж�ʱ��
            if (!queryParam["sTime"].IsEmpty() && !queryParam["eTime"].IsEmpty())
            {
                string sTime = queryParam["sTime"].ToString() + " 00:00:00";
                string eTime = queryParam["eTime"].ToString() + " 23:59:59";

                pagination.p_fields = string.Format(@"v_info.NATURE,v_info.DEPTNAME,v_info.DEPARTMENTCODE,v_info.ORGANIZEID,row_number() over(partition by case when v_info.nature = '����' then v_info.parentname else v_info.DEPTNAME end order by v_info.REALNAME asc) as shu,
                    case when (length(v_info.deptcode)>20) then (select d.SORTCODE from base_department d where d.deptcode = substr(v_info.deptcode,1,20)) else v_info.DEPTSORT end as DEPTSORTss,case when v_info.nature = '����' then v_info.parentname else v_info.DEPTNAME end as bmname
                                    ,v_info.realname,v_info.dutyname,(select count(b.userid) from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) b where b.userid = v_info.userid and b.inout = 0 and 
                                    b.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and b.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss')) as intnum,(select count(c.userid) from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) c where c.userid = v_info.userid and c.inout = 1
                                    and c.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and c.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss')) as outnum,d.depttype,'' as bminnum,'' as bmoutnum", sTime, eTime);
            }


            //��������Code��ѯ����
            if (!queryParam["deptCode"].IsEmpty())
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string deptCode = queryParam["deptCode"].ToString();
                string deptType = "";
                if (deptCode.StartsWith("cx100_"))
                {
                    deptCode = deptCode.Replace("cx100_", "");
                    deptType = "��Э";
                }
                if (deptCode.StartsWith("ls100_"))
                {
                    deptCode = deptCode.Replace("ls100_", "");
                    deptType = "��ʱ";
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
            //�ж����ڲ���λ���������λ
            if (!queryParam["state"].IsEmpty())
            {
                string state = queryParam["state"].ToString();
                string RoleName = curuser.RoleName;
                string sTime = queryParam["sTime"].ToString() + " 00:00:00";
                string eTime = queryParam["eTime"].ToString() + " 23:59:59";

                if (state == "0")
                {
                    pagination.conditionJson += string.Format(" and v_info.nature != '�а���'");

                    if (RoleName.Contains("���������û�") || RoleName.Contains("��˾�쵼") || RoleName.Contains("��˾����Ա") || RoleName.Contains("��˾���û�") || RoleName.Contains("��������Ա") || curuser.UserId.Contains("1521c21a-62c9-4aa1-9093-a8bda503ea89"))
                    {
                        //�˼�����û��ɲ鿴��������
                        pagination.p_fields += string.Format(@",(select COUNT(a.USERID) as num from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) a left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 0 and 
                            a.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and a.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss') and (case when b.depttype is null then '�ڲ�' else b.depttype end) = '�ڲ�' {2}) as allintnum,
                            (select COUNT(a.USERID) as num from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) a left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 1 and 
                            a.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and a.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss') and (case when b.depttype is null then '�ڲ�' else b.depttype end) = '�ڲ�' {2}) as alloutnum", sTime, eTime, sqlint_out);
                    }
                    else
                    {
                        pagination.p_fields += string.Format(@",(select COUNT(a.USERID) as num from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) a left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 0 and 
                            a.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and a.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss') and (case when b.depttype is null then '�ڲ�' else b.depttype end) = '�ڲ�' 
                            and (case when (b.ISEPIBOLY = '��' and b.DEPTTYPE is null and length(b.deptcode)>20) then (select fullname from base_department d where d.deptcode = substr(b.deptcode,1,20)) else b.DEPTNAME end) = '{2}' {3}) as allintnum,
                            (select COUNT(a.USERID) as num from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) a left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 1 and 
                            a.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and a.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss') and (case when b.depttype is null then '�ڲ�' else b.depttype end) = '�ڲ�'
                            and (case when (b.ISEPIBOLY = '��' and b.DEPTTYPE is null and length(b.deptcode)>20) then (select fullname from base_department d where d.deptcode = substr(b.deptcode,1,20)) else b.DEPTNAME end) = '{2}' {3}) as alloutnum", sTime, eTime, bmname, sqlint_out);
                    }
                }
                else
                {
                    pagination.conditionJson += string.Format(" and v_info.nature = '�а���'");
                    pagination.sidx = string.Format("v_info.DEPTTYPE,v_info.DEPTSORT,v_info.DEPTNAME,v_info.DEPTCODE,v_info.USERID");
                    pagination.sord = string.Format("desc");
                    
                    if (RoleName.Contains("���������û�") || RoleName.Contains("��˾�쵼") || RoleName.Contains("��˾����Ա") || RoleName.Contains("��˾���û�") || RoleName.Contains("��������Ա") || curuser.UserId.Contains("1521c21a-62c9-4aa1-9093-a8bda503ea89"))
                    {
                        //�˼�����û��ɲ鿴��������
                        pagination.p_fields += string.Format(@",(select COUNT(a.USERID) as num from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) a   left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 0 and 
                            a.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and a.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss') and (case when b.depttype is null then '�ڲ�' else b.depttype end) != '�ڲ�' {2}) as allintnum,
                            (select COUNT(a.USERID) as num from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) a left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 1 and 
                            a.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and a.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss') and (case when b.depttype is null then '�ڲ�' else b.depttype end) != '�ڲ�'{2}) as alloutnum", sTime, eTime, sqlint_out);
                    }
                    else
                    {
                        
                        pagination.p_fields += string.Format(@",(select COUNT(a.USERID) as num from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) a left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 0 and 
                            a.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and a.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss') and (case when b.depttype is null then '�ڲ�' else b.depttype end) != '�ڲ�' 
                            and (case when (b.ISEPIBOLY = '��' and b.DEPTTYPE is null and length(b.deptcode)>20) then (select fullname from base_department d where d.deptcode = substr(b.deptcode,1,20)) else b.DEPTNAME end) = '{2}' {3}) as allintnum,
                            (select COUNT(a.USERID) as num from (select DISTINCT userid,inout,devicename,CREATEDATE from bis_hikinoutlog) a left join V_USERINFO b on a.USERID = b.USERID where b.DEPTNAME is not null and a.INOUT = 1 and 
                            a.CREATEDATE >= to_date('{0}','yyyy-MM-dd hh24:mi:ss') and a.CREATEDATE <= to_date('{1}','yyyy-MM-dd hh24:mi:ss') and (case when b.depttype is null then '�ڲ�' else b.depttype end) != '�ڲ�'
                            and (case when (b.ISEPIBOLY = '��' and b.DEPTTYPE is null and length(b.deptcode)>20) then (select fullname from base_department d where d.deptcode = substr(b.deptcode,1,20)) else b.DEPTNAME end) = '{2}' {3}) as alloutnum", sTime, eTime, bmname, sqlint_out);
                    }
                }
            }
            //�жϵ�λ����
            if (!queryParam["deptType"].IsEmpty())
            {
                string deptType = queryParam["deptType"].ToString();
                string state = queryParam["state"].ToString();
                if (state == "1")
                {
                    pagination.conditionJson += string.Format(" and d.depttype = '{0}'", deptType);
                }
            }
            //�ж���������
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
            //�û�����
            if (!queryParam["Name"].IsEmpty())
            {
                string PersonName = queryParam["Name"].ToString();
                pagination.conditionJson += string.Format(" and v_info.REALNAME like '%{0}%'", PersonName);
            }
            
            //�жϵ�ǰ��½�û���ʲô����
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
                    if (RoleName.Contains("�а��̼��û�"))
                    {
                        //�а��̼��û�ֻ�ɲ鿴����λ�Ž�����
                        //var bmname = GetBmname(curuser.UserId);
                        pagination.conditionJson += string.Format(" and (t.ISREFER is NULL or t.userid = '{0}')", curuser.UserId);
                        pagination.conditionJson += string.Format(" and v_info.ROLENAME like '%{0}%' and v_info.DEPTNAME = '{1}'", RoleName, bmname);
                    }
                    else if (RoleName.Contains("���������û�") || RoleName.Contains("��˾�쵼") || RoleName.Contains("��˾����Ա") || RoleName.Contains("��˾���û�") || RoleName.Contains("��������Ա") || curuser.UserId.Contains("1521c21a-62c9-4aa1-9093-a8bda503ea89"))
                    {
                        //�˼�����û��ɲ鿴��������
                    }
                    else
                    {
                        //var bmname = GetBmname(curuser.UserId);
                        pagination.conditionJson += string.Format(" and (t.ISREFER is NULL or t.userid = '{0}')", curuser.UserId);
                        pagination.conditionJson += string.Format(" and (v_info.ROLENAME like '%�а��̼��û�%' or v_info.DEPTNAME = '{0}' or v_info.PARENTNAME = '{0}')", bmname);
                    }
                }
                
            }
            int num = pagination.p_fields.Length;
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// �����û�ID��ȡ��������
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetBmname(string userId)
        {
            string sql = string.Format(@"select case when a.nature = '����' then a.parentname else a.DEPTNAME end as bmname 
                                                from v_userinfo a where a.USERID = '{0}'", userId);

            var result = this.BaseRepository().FindTable(sql);

            return result.Rows[0]["bmname"].ToString();
        }

        /// <summary>
        /// ��ȡ��Ա�Ž�������ϸ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTableByUserID(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //�����û�ID�鿴�Ž�������ϸ��¼
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
            //�鿴������״̬
            if (!queryParam["inOut"].IsEmpty())
            {
                string inOut = queryParam["inOut"].ToString();

                pagination.conditionJson += string.Format(" and inout = '{0}'", inOut);
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// ��ȡ��Ա���ñ�����
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
        /// ��ȡ������������
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

        #region �ύ����
        /// <summary>
        /// ��Աͨ���ύ
        /// </summary>
        /// <param name="insert"></param>
        /// <param name="update"></param>
        public void UserAisleSave(HikinoutlogEntity insert, HikinoutlogEntity update)
        {
            //��ʼ����
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
        /// ��������ID�޸��볡״̬
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
        /// ������Ա����IDɾ������
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
        /// ��������������Ա���ɲ�ѯ�Ž�
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

            //�糧������
            var data11 = this.BaseRepository().IQueryable().Where(x => x.CreateDate >= from && x.CreateDate < to && x.AreaName == "һ�Ÿ�" && x.DeviceType == 0 && x.IsOut == 0 && x.UserType == 0).Count();
            //�糧������
            var data12 = this.BaseRepository().IQueryable().Where(x => x.CreateDate >= from && x.CreateDate < to && x.AreaName == "���Ÿ�" && x.DeviceType == 0 && x.IsOut == 0 && x.UserType == 0).Count();
            //�糧��ͷ��
            var data13 = this.BaseRepository().IQueryable().Where(x => x.CreateDate >= from && x.CreateDate < to && x.AreaName == "��ͷ��" && x.DeviceType == 0 && x.IsOut == 0 && x.UserType == 0).Count();
            //�ϼ�
            //data11 = 140;
            //data12 = 26;
            var data14 = data11 + data12 + data13;
            //��ί������
            var data21 = this.BaseRepository().IQueryable().Where(x => x.CreateDate >= from && x.CreateDate < to && x.AreaName == "һ�Ÿ�" && x.DeviceType == 0 && x.IsOut == 0 && x.UserType == 1).Count();
            //��ί������
            var data22 = this.BaseRepository().IQueryable().Where(x => x.CreateDate >= from && x.CreateDate < to && x.AreaName == "���Ÿ�" && x.DeviceType == 0 && x.IsOut == 0 && x.UserType == 1).Count();
            //��ί��ͷ��
            var data23 = this.BaseRepository().IQueryable().Where(x => x.CreateDate >= from && x.CreateDate < to && x.AreaName == "��ͷ��" && x.DeviceType == 0 && x.IsOut == 0 && x.UserType == 1).Count();
            //�ϼ�
            //data21 = 324;
            //data22 = 55;
            var data24 = data21 + data22 + data23;
            //��ʱ������
            var data31 = this.BaseRepository().IQueryable().Where(x => x.CreateDate >= from && x.CreateDate < to && x.AreaName == "һ�Ÿ�" && x.DeviceType == 0 && x.IsOut == 0 && x.UserType == 2).Count();
            //��ʱ������
            var data32 = this.BaseRepository().IQueryable().Where(x => x.CreateDate >= from && x.CreateDate < to && x.AreaName == "���Ÿ�" && x.DeviceType == 0 && x.IsOut == 0 && x.UserType == 2).Count();
            //��ʱ��ͷ��
            var data33 = this.BaseRepository().IQueryable().Where(x => x.CreateDate >= from && x.CreateDate < to && x.AreaName == "��ͷ��" && x.DeviceType == 0 && x.IsOut == 0 && x.UserType == 2).Count();
            //�ϼ�
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
                              where q2.ItemName == "�����Ž���ӳ��"
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
        /// ��ȡ�������Ա��������
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetTodayCarPeopleCount()
        {
            //!!!��̩�糧 1�Ÿڡ�2�Ÿ�Ϊ�������볧ͨ���������Ÿڽ���������ͳ��֮��
            DateTime startTime = DateTime.Now.Date;
            DateTime endTime = startTime.AddDays(1);
            string sql = string.Format(@" 
	 SELECT * FROM (SELECT COUNT(*) AS PEOPLEIN FROM BIS_HIKINOUTLOG WHERE INOUT=0 AND CREATEDATE>TO_DATE('{0}', 'yyyy-MM-dd') AND CREATEDATE<TO_DATE('{1}', 'yyyy-MM-dd') and DEVICEHIKID in (select ITEMVALUE from BASE_DATAITEMDETAIL where itemid in ('a665b9a3-f24a-4876-ae0e-8830336f151c','8ee4c49e-8b4e-499e-96ab-6a1db80ab36c'))) ,
  (SELECT COUNT(*)  AS PEOPLEOUT FROM BIS_HIKINOUTLOG WHERE INOUT=1 AND CREATEDATE>TO_DATE('{0}', 'yyyy-MM-dd') AND CREATEDATE<TO_DATE('{1}', 'yyyy-MM-dd') and DEVICEHIKID in (select ITEMVALUE from BASE_DATAITEMDETAIL where itemid in ('a665b9a3-f24a-4876-ae0e-8830336f151c','8ee4c49e-8b4e-499e-96ab-6a1db80ab36c'))),
	(SELECT  COUNT(*)  AS CARIN FROM BIS_CARINLOG WHERE STATUS=0 AND CREATEDATE>TO_DATE('{0}', 'yyyy-MM-dd') AND CREATEDATE<TO_DATE('{1}', 'yyyy-MM-dd') and (ADDRESS like '%2#��ͤ%' OR ADDRESS LIKE '%1#��ͤ%')),
	(SELECT  COUNT(*)  AS CAROUT FROM BIS_CARINLOG WHERE STATUS=1  AND CREATEDATE>TO_DATE('{0}', 'yyyy-MM-dd') AND CREATEDATE<TO_DATE('{1}', 'yyyy-MM-dd') and (ADDRESS like '%2#��ͤ%' OR ADDRESS LIKE '%1#��ͤ%'))", startTime.ToString("yyyy-MM-dd"), endTime.ToString("yyyy-MM-dd"));
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
        /// ��ȡ���µĳ�����Ա�������ݣ�ȡǰ����
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetCarPeopleTopData()
        {
            var db = new RepositoryFactory().BaseRepository();
            var peopleList = db.IQueryable<HikinoutlogEntity>().OrderByDescending(x => x.CreateDate).Select(x => new
            {
                x.UserName,
                x.CreateDate,
                InOut = x.InOut == 1 ? "�뿪" : "����",
                x.AreaName
            }).Take(5).ToList();

            var carList = db.IQueryable<CarinlogEntity>().OrderByDescending(x => x.CreateDate).Select(x => new
            {
                x.DriverName,
                x.CreateDate,
                InOut = x.Status == 1 ? "�뿪" : "����",
                x.CarNo,
                x.Address
            }).Take(5).ToList();
            Dictionary<string, object> dic = new Dictionary<string, object> { { "People", peopleList }, { "Car", carList } };
            return dic;

        }

        #endregion
    }
}
