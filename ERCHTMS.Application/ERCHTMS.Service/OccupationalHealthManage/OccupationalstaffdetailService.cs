using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.IService.OccupationalHealthManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
using ERCHTMS.Code;


namespace ERCHTMS.Service.OccupationalHealthManage
{
    /// <summary>
    /// �� ����ְҵ���������
    /// </summary>
    public class OccupationalstaffdetailService : RepositoryFactory<OccupationalstaffdetailEntity>, OccupationalstaffdetailIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OccupationalstaffdetailEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// ���ݹ���id�����޸����ʱ��
        /// </summary>
        /// <param name="time">���ʱ��</param>
        /// <param name="parenid">����id</param>
        /// <returns></returns>
        public int UpdateTime(DateTime time, string parenid)
        {
            //return this.BaseRepository().ExecuteBySql(DbParameters.FormatSql("update BIS_OCCUPATIONALSTAFFDETAIL set INSPECTIONTIME=:INSPECTIONTIME where OCCID=:OCCID"), new DbParameter[] { DbParameters.CreateDbParameter(":INSPECTIONTIME", time), DbParameters.CreateDbParameter(":OCCID", parenid) });
            return this.BaseRepository().ExecuteBySql(string.Format("UPDATE BIS_OCCUPATIONALSTAFFDETAIL SET INSPECTIONTIME=to_timestamp('{0}','yyyy-mm-dd hh24:mi:ss') where OCCID='{1}'", time, parenid));
        }

        /// <summary>
        /// ����������ѯ��������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTable(string queryJson, string where)
        {
            string Sql = "SELECT row_number() over(ORDER BY INSPECTIONTIME DESC) as IDNUM,OCCDETAILID,USERNAME,USERNAMEPINYIN,TO_CHAR(INSPECTIONTIME,'yyyy-mm-dd hh24:mi:ss')as INSPECTIONTIME,ISSICK,SICKTYPENAME as SICKTYPE,CASE WHEN ISSICK =1 THEN '��' ELSE '��' END AS ISENDEMIC,CASE WHEN ISSICK =2 THEN '��' ELSE '��' END AS ISUNUSUAL,UNUSUALNOTE  FROM BIS_OCCUPATIONALSTAFFDETAIL WHERE 1=1";
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();

                //��ID
                Sql += string.Format(" and OCCID  like '%{0}%'", condition.Trim());
                if (keyord.ToInt() == 1) //��ѯְҵ��
                {
                    //Type
                    Sql += string.Format(" and ISSICK like {0}", keyord.Trim());
                }
                else if (keyord.ToInt() == 2)  //��ѯ�쳣��Ա
                {
                    Sql += string.Format(" and ISSICK like {0}", keyord.Trim());
                }
                else if (keyord.ToInt() == 4)  //��ѯ���ǽ�����Ա
                {
                    Sql += string.Format(" and ISSICK != 4");
                }
                //if (keyord.ToInt() < 2)//����2ʱ��ѯȫ��
                //{
                //    //Type
                //    Sql += string.Format(" and ISSICK like {0}", keyord.Trim());
                //}

            }
            Sql += where;

            Sql += " ORDER BY INSPECTIONTIME DESC";
            return this.BaseRepository().FindTable(Sql);
        }

        /// <summary>
        /// ��ȡ�û�id�µ���������¼
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetUserTable(string userid)
        {
            string Sql = "SELECT USERNAME,INSPECTIONTIME,case when ISSICK=0 then '��' else '��' end issick,ITEMNAME,UNUSUALNOTE,MECHANISMNAME FROM V_USERSTAFF WHERE 1=1";

            if (userid.Trim() != "")
            {
                Sql += string.Format(" and USERID  = '{0}'", userid.Trim());
            }
            //Sql += where;
            return this.BaseRepository().FindTable(Sql);
        }

        /// <summary>
        /// ��ȡ�û�id�µ���������¼ ��
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable NewGetUserTable(string userid)
        {
            string sql =
                @"SELECT st.USERNAME,st.INSPECTIONTIME,ITEMNAME,st.mechanismname,staff.occid,isannex,filenum,detail.UNUSUALNOTE,CASE WHEN DETAIL.ISSICK =1 THEN '��' ELSE '��' END AS ISENDEMIC,CASE WHEN DETAIL.ISSICK =2 THEN '��' ELSE '��' END AS ISUNUSUAL FROM V_USERSTAFF  st
            left join bis_occupationalstaffdetail  detail on st.occdetailid=detail.occdetailid
            left join V_OCCUPATIOALSTAFF staff on detail.occid=staff.occid WHERE 1=1 ";
            if (userid.Trim() != "")
            {
                sql += string.Format(" and st.USERID  = '{0}'", userid.Trim());
            }
            //Sql += where;
            return this.BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// ��ȡ�û��ĽӴ�Σ������
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserHazardfactor(string useraccount)
        {
            string sql = string.Format(
                @"select LISTAGG(riskvalue,',') WITHIN GROUP (ORDER BY riskvalue) AS us from (              
            select userid,username,riskvalue from BIS_HAZARDFACTORUSER hauser
            left join BIS_HAZARDFACTORs ha on ha.hid=hauser.hid
             where 1=1
             group by userid,username,riskvalue) f where userid='{0}'  group by userid,username", useraccount);
            DataTable ret = BaseRepository().FindTable(sql);
            return ret;

        }

        /// <summary>
        /// ��ȡְҵ��ͳ�Ʊ�
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetStatisticsUserTable(string year, string where)
        {
            string Sql = @"select DDE.ITEMNAME,count(DDE.ITEMNAME) as itemsum,OCC.SICKTYPE from BIS_OCCUPATIONALSTAFFDETAIL OCC
                            LEFT JOIN BASE_DATAITEMDETAIL DDE on OCC.Sicktype=DDE.ITEMVALUE
                            LEFT JOIN Base_Dataitem DI ON DI.ITEMID=DDE.Itemid
                            LEFT JOIN BIS_OCCUPATIOALSTAFF OC ON OCC.OCCID=OC.OCCID
                            where DI.ITEMCODE='SICKTYPE' and OC.MECHANISMNAME is not null";

            if (year != "")
            {
                Sql += string.Format(" and to_char(OC.INSPECTIONTIME,'yyyy')='{0}'", year.Trim());
            }
            Sql += " and OCC." + where;
            Sql += string.Format("  group by DDE.ITEMNAME,OCC.SICKTYPE");



            return this.BaseRepository().FindTable(Sql);
        }

        /// <summary>
        /// ��ȡ�µ�ְҵ��ͳ�Ʊ�
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable NewGetStatisticsUserTable(string year, string where)
        {
            //�Ȼ�ȡ����һ��ְҵ����Ϣ
            string Sql = @"select DDE.ITEMNAME,count(DDE.ITEMNAME) as count,DDE.ITEMVALUE from BASE_DATAITEMDETAIL DDE
                            LEFT JOIN Base_Dataitem DI ON DI.ITEMID=DDE.Itemid
                            LEFT JOIN (select SUBSTR(DDE.ITEMVALUE,0,3) ITEMVALUE from BASE_DATAITEMDETAIL DDE
                            LEFT JOIN Base_Dataitem DI ON DI.ITEMID=DDE.Itemid
                            LEFT JOIN BIS_OCCUPATIONALSTAFFDETAIL OCC ON instr(OCC.Sicktype,DDE.ITEMVALUE)>0
                            LEFT JOIN BIS_OCCUPATIOALSTAFF OC ON OCC.OCCID=OC.OCCID
                            where DI.ITEMCODE='SICKTYPE' and length(DDE.ITEMVALUE)>3
                            and OC.MECHANISMNAME is not null
                            ";

            if (year != "")
            {
                Sql += string.Format(" and to_char(OC.INSPECTIONTIME,'yyyy')='{0}'", year.Trim());
            }
            Sql += " and OCC." + where;
            Sql += string.Format(" group by SUBSTR(DDE.ITEMVALUE,0,3),OCC.Userid");//��һ���������ͳ�Ƶò���������ͳ�Ƶò����� ע�;���ͳ�Ƶò�����
            Sql += string.Format(@" ) DT ON DT.Itemvalue=DDE.Itemvalue
                            where DI.ITEMCODE='SICKTYPE' and length(DDE.ITEMVALUE)=3 and DT.ITEMVALUE is not null");

            Sql += string.Format("  group by DDE.ITEMNAME,DDE.Sortcode,DDE.ITEMVALUE  order by DDE.Sortcode");



            return this.BaseRepository().FindTable(Sql);
        }

        /// <summary>
        /// ��ȡ����ְҵ��ͳ�Ʊ�
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetStatisticsDeptTable(string year, string where)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string Sql = @"select Dep.Fullname, Count(Dep.Fullname) as DepSum,Dep.Encode from Base_Department Dep
                            LEFT JOIN Base_User U ON DEP.Encode =U.Departmentcode
                            LEFT JOIN BIS_OCCUPATIONALSTAFFDETAIL OSD ON OSD.Userid=U.Userid
                            LEFT JOIN BIS_OCCUPATIOALSTAFF OC ON OSD.OCCID=OC.OCCID
                            where DEP.OrganizeId='" + user.OrganizeId + "' and OC.MECHANISMNAME is not null and OSD.Issick=1";

            if (year != "")
            {
                Sql += string.Format(" and to_char(OC.INSPECTIONTIME,'yyyy')='{0}'", year.Trim()); 
            }
            Sql += " and OC." + where;
            Sql += string.Format("   group by Dep.Encode,Dep.Fullname");



            return this.BaseRepository().FindTable(Sql);
        }

        /// <summary>
        /// ��ȡ���ְҵ��ͳ�Ʊ�
        /// </summary>
        /// <param name="yearType">����������</param>
        /// <param name="Dept">����EnCode</param>
        /// <returns></returns>
        public DataTable GetStatisticsYearTable(int yearType, string Dept, string where)
        {
            int year = DateTime.Now.Year;
            string Sql = @"select to_char(OC.INSPECTIONTIME,'yyyy') as year ,COUNT(OC.INSPECTIONTIME) from BIS_OCCUPATIONALSTAFFDETAIL OSD
                            LEFT JOIN BIS_OCCUPATIOALSTAFF OC ON OSD.OCCID=OC.OCCID
                            LEFT JOIN Base_User U ON U.Userid=OSD.Userid
                            LEFT JOIN Base_Department Dep  ON DEP.Encode =SUBSTR(U.Departmentcode,0,6)
                            where OC.MECHANISMNAME is not null  and OSD.Issick=1";
            if (yearType != 0)//�����0���ȫ��
            {
                Sql += string.Format("  AND OC.INSPECTIONTIME  >= TO_DATE('{0}','yyyy') AND OC.INSPECTIONTIME<=TO_DATE('{1}','yyyy-mm-dd hh24:mi:ss')", (year - yearType), year + "-12-31 23:59:59");
            }
            if (Dept != null && Dept != "")
            {
                Sql += string.Format(" AND DEP.Encode like'{0}%'", Dept);
            }
            Sql += " AND OC." + where;
            Sql += string.Format("  group by to_char(OC.INSPECTIONTIME,'yyyy') order by to_char(OC.INSPECTIONTIME,'yyyy') asc");
            return this.BaseRepository().FindTable(Sql);
        }

        /// <summary>
        /// ��ȡְҵ����Ա�嵥(ȫ����)
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetStaffList(string queryJson, string where)
        {
            string Sql = "SELECT OCCDETAILID,USERID,USERNAME,GENDER,DEPTNAME,DUTYNAME,ISSICK,SICKTYPE,SICKTYPENAME FROM V_STAFFDETAILLIST WHERE ISSICK=1";
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "UserName":          //����
                        Sql += string.Format(" and USERNAME  like '%{0}%'", keyord);
                        break;
                    default:
                        break;
                }
            }

            //��ѯ�൱����һ������ӵ�ְҵ���� ����������ȵ���һ�����ɸѡ
            if (!queryParam["type"].IsEmpty())
            {
                string type = queryParam["type"].ToString();
                if (type == "0")
                {   //ֻ��ѯ������������
                    Sql += string.Format(" and Userid not in(select userid from BIS_OCCUPATIONALSTAFFDETAIL D LEFT JOIN BIS_OCCUPATIOALSTAFF OCC ON D.OCCID=OCC.OCCID where OCC.MECHANISMNAME is not null and to_char(D.INSPECTIONTIME,'yyyy')='{0}' and ISSICK=1)", DateTime.Now.Year - 1);
                }
                else
                {
                    if (!queryParam["time"].IsEmpty())
                    {
                        string time = queryParam["time"].ToString();
                        Sql += string.Format(" and to_char(INSPECTIONTIME,'yyyy') = '{0}'", time);
                    }
                }

            }

            Sql += where;

            Sql += string.Format(" order by INSPECTIONTIME desc ,Usernamepinyin asc ");
            return this.BaseRepository().FindTable(Sql);
        }


        /// <summary>
        /// ��ȡְҵ����Ա�嵥��Ա����
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public int GetStaffListSum(string queryJson, string where)
        {
            string Sql = "SELECT Count(OCCDETAILID)as count FROM V_STAFFDETAILLIST WHERE ISSICK=1";
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "UserName":          //����
                        Sql += string.Format(" and USERNAME  like '%{0}%'", keyord);
                        break;
                    case "SickType":        //ְҵ��
                        Sql += string.Format(" and SICKTYPE  Like '%{0}%'", keyord);
                        break;
                    case "DeptCode":        //���ű���
                        Sql += string.Format(" and ENCODE  like '{0}%'", keyord);
                        break;
                    default:
                        break;
                }
            }

            //��ѯ�൱����һ������ӵ�ְҵ���� ����������ȵ���һ�����ɸѡ
            if (!queryParam["type"].IsEmpty())
            {
                string type = queryParam["type"].ToString();
                if (type == "0")
                {   //ֻ��ѯ������������
                    Sql += string.Format(" and Userid not in(select userid from BIS_OCCUPATIONALSTAFFDETAIL D LEFT JOIN BIS_OCCUPATIOALSTAFF OCC ON D.OCCID=OCC.OCCID where OCC.MECHANISMNAME is not null and to_char(D.INSPECTIONTIME,'yyyy')='{0}' and ISSICK=1) and to_char(INSPECTIONTIME,'yyyy')='{1}'", DateTime.Now.Year - 1, DateTime.Now.Year);
                }
                else
                {
                    if (!queryParam["time"].IsEmpty())
                    {
                        string time = queryParam["time"].ToString();
                        Sql += string.Format(" and to_char(INSPECTIONTIME,'yyyy') = '{0}'", time);
                    }
                }

            }
            Sql += where;

            Sql += string.Format(" order by INSPECTIONTIME desc ,Usernamepinyin asc ");

            object obj = this.BaseRepository().FindObject(Sql);

            return obj.ToInt(); ;
        }

        /// <summary>
        /// ��ȡְҵ����Ա�嵥
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetStaffListPage(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "Account":            //�˻�
                        pagination.conditionJson += string.Format(" and ACCOUNT  like '%{0}%'", keyord);
                        break;
                    case "UserName":          //����
                        pagination.conditionJson += string.Format(" and USERNAME  like '%{0}%'", keyord);
                        break;
                    case "Mobile":          //�ֻ�
                        pagination.conditionJson += string.Format(" and MOBILE like '%{0}%'", keyord);
                        break;
                    case "SickType":        //ְҵ��
                        pagination.conditionJson += string.Format(" and SICKTYPE  Like '%{0}%'", keyord);
                        break;
                    case "DeptCode":        //���ű���
                        pagination.conditionJson += string.Format(" and ENCODE  like '{0}%'", keyord);
                        break;
                    default:
                        break;
                }
            }

            //��ѯ�൱����һ������ӵ�ְҵ���� ����������ȵ���һ�����ɸѡ
            if (!queryParam["type"].IsEmpty())
            {
                string type = queryParam["type"].ToString();
                if (type == "0")
                {
                    //ֻ��ѯ������������
                    pagination.conditionJson += string.Format(" and Userid not in(select userid from BIS_OCCUPATIONALSTAFFDETAIL D LEFT JOIN BIS_OCCUPATIOALSTAFF OCC ON D.OCCID=OCC.OCCID where OCC.MECHANISMNAME is not null and to_char(D.INSPECTIONTIME,'yyyy')='{0}' and ISSICK=1) and to_char(INSPECTIONTIME,'yyyy')='{1}'", DateTime.Now.Year - 1, DateTime.Now.Year);
                }
                else
                {
                    if (!queryParam["time"].IsEmpty())
                    {
                        string time = queryParam["time"].ToString();
                        pagination.conditionJson += string.Format(" and to_char(INSPECTIONTIME,'yyyy') = '{0}'", time);
                    }
                }

            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// ��������ɾ�������û� �������
        /// </summary>
        /// <param name="SickType">�Ƿ���ְҵ��</param>
        /// <param name="parenid">����id</param>
        /// <returns></returns>
        public int Delete(string parenid, int SickType)
        {
            return this.BaseRepository().ExecuteBySql(string.Format("DELETE BIS_OCCUPATIONALSTAFFDETAIL where OCCID='{0}' and ISSICK={1}", parenid, SickType));
        }


        /// <summary>
        /// ���ݸ�id���Ƿ�������ѯ������Ϣ
        /// </summary>
        /// <param name="Pid">��id</param>
        /// <param name="SickType">�Ƿ�����</param>
        /// <returns></returns>
        public IEnumerable<OccupationalstaffdetailEntity> GetList(string Pid, int Issick)
        {
            //����1��2��ѯ��ְҵ�����쳣����Ա
            if (Issick ==1 || Issick==2)
            {
                return this.BaseRepository().IQueryable(it => it.OccId == Pid && it.Issick == Issick).ToList();
            }
            else if (Issick ==4)
            {
                return this.BaseRepository().IQueryable(it => it.OccId == Pid && (it.Issick == 1 || it.Issick == 2));
            }
            else
            {
                return this.BaseRepository().IQueryable(it => it.OccId == Pid).ToList();
            }
        }


        /// <summary>
        /// �洢���̷�ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageListByProc(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();

                //��ID
                pagination.conditionJson += string.Format(" and OCCID  like '%{0}%'", condition.Trim());
                if (keyord.ToInt() == 1) //��ѯְҵ��
                {
                    //Type
                    pagination.conditionJson += string.Format(" and ISSICK like {0}", keyord.Trim());
                }
                else if (keyord.ToInt() == 2)  //��ѯ�쳣��Ա
                {
                    pagination.conditionJson += string.Format(" and ISSICK like {0}", keyord.Trim());
                }
                else if (keyord.ToInt() ==4)
                {
                    pagination.conditionJson += string.Format(" and ISSICK != 4");
                }


            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OccupationalstaffdetailEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region �ύ����
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
        public void SaveForm(string keyValue, OccupationalstaffdetailEntity entity)
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
        #endregion
    }
}
