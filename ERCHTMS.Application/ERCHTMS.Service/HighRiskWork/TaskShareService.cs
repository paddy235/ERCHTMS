using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System;
using ERCHTMS.Code;
using System.Data.Common;
using ERCHTMS.Entity.BaseManage;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ERCHTMS.Service.SystemManage;
using System.Net;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public class TaskShareService : RepositoryFactory<TaskShareEntity>, TaskShareIService
    {
        private IDepartmentService Idepartmentservice = new DepartmentService();
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TaskShareEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TaskShareEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ���������б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetDataTable(Pagination page, string queryJson, string authType)
        {
            DatabaseType dataTye = DatabaseType.Oracle;
            #region ���
            page.p_kid = "a.id";
            page.p_fields = @"a.tasktype,a.supervisedeptname,a.supervisedeptcode,a.createusername,a.createdate,a.flowstep,a.createuserid,a.issubmit,a.supervisedeptid,a.flowrolename,a.flowdept,a.flowdeptname,b.fullname";
            page.p_tablename = @"bis_taskshare a left join base_department b on  a.createuserdeptcode=b.encode";
            #endregion
            #region ����Ȩ��
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                if (!string.IsNullOrEmpty(authType))
                {
                    switch (authType)
                    {
                        case "1":
                            page.conditionJson += " and a.createuserid='" + user.UserId + "'";
                            break;
                        case "2":
                            page.conditionJson += " and a.createuserdeptcode='" + user.DeptCode + "";
                            break;
                        case "3":
                            page.conditionJson += string.Format(@" and ((a.supervisedeptcode like  '{0}%' ) or (a.id in(select distinct(taskshareid) from  (select taskshareid from bis_teamsinfo where teamcode like '{0}%' union select taskshareid from bis_staffinfo where pteamcode like '{0}%' union select taskshareid from bis_staffinfo where  pteamid in(select  distinct outprojectid from epg_outsouringengineer where engineerletdeptid='{1}')))))", user.DeptCode,user.DeptId);
                            break;
                        case "4":
                            page.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                            break;
                        case "app":
                            if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
                            {
                                page.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                            }
                            else
                            {
                                page.conditionJson += string.Format(@" and ((a.supervisedeptcode like  '{0}%' ) or (a.id in(select distinct(taskshareid) from  (select taskshareid from bis_teamsinfo where teamcode like '{0}%' union select taskshareid from bis_staffinfo where pteamcode like '{0}%') union select taskshareid from bis_staffinfo where  pteamid in(select  distinct outprojectid from epg_outsouringengineer where engineerletdeptid='{1}'))))", user.DeptCode,user.DeptId);
                            }
                            break;
                    }
                }
                else
                {
                    page.conditionJson = " and 0=1";
                }
            }
            #endregion
            #region  ɸѡ����
            var queryParam = queryJson.ToJObject();
            //��վ�ල��λ
            if (!queryParam["SuperviseDeptCode"].IsEmpty())
            {
                page.conditionJson += string.Format(" and SuperviseDeptCode='{0}'", queryParam["SuperviseDeptCode"].ToString());
            }
            //������λ
            if (!queryParam["CreateUserDeptCode"].IsEmpty())
            {
                page.conditionJson += string.Format(" and a.CreateUserDeptCode='{0}'", queryParam["CreateUserDeptCode"].ToString());
            }
            //������
            if (!queryParam["searchtype"].IsEmpty())
            {
                string[] roles = user.RoleName.Split(',');
                string roleWhere = "";
                foreach (var r in roles)
                {
                    roleWhere += string.Format("or a.flowrolename like '%{0}%'", r);
                }
                roleWhere = roleWhere.Substring(2);
                //��ǰ�����Ȩ�޵Ĳ��ż���ɫ���ſɲ鿴
                page.conditionJson += string.Format("  and a.flowdept like '%{0}%' and ({1})", user.DeptId, roleWhere);
            }
            //�ų������˱�������
            page.conditionJson += string.Format("  and a.id  not in(select id from bis_taskshare where issubmit='0' and  createuserid!='{0}' and flowdept is null)", user.UserId);
            #endregion

            return this.BaseRepository().FindTableByProcPager(page, dataTye);
        }

        #region ͳ��
        /// <summary>
        /// ��վ�ලͳ��
        /// </summary>
        /// <param name="sentity"></param>
        /// <returns></returns>
        public DataTable QueryStatisticsByAction(StatisticsEntity sentity)
        {
            string sql = string.Empty;

            string str = " 1=1";


            Operator cuUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            DepartmentEntity dept = Idepartmentservice.GetEntityByCode(sentity.sDeptCode);

            if (dept.Nature == "����")
            {
                sentity.isCompany = true;
            }
            else
            {
                sentity.isCompany = false;
            }
            
            //��ʼ����
            if (!sentity.startDate.IsEmpty())
            {
                str += string.Format(" and pstarttime>=to_date('{0}','yyyy-mm-dd')", sentity.startDate);
            }
            //��ֹ����
            if (!sentity.endDate.IsEmpty())
            {
                str += string.Format(" and pendtime<=to_date('{0}','yyyy-mm-dd')", sentity.endDate);
            }
            switch (sentity.sAction)
            {

                //�ල�����б�
                case "1":
                    #region �ල�����б�
                    if (sentity.sMark == 0) //�����ල�Ա�ͼ��ѯ
                    {
                        //����
                        if (sentity.isCompany)
                        {
                            sql = string.Format(@"select a.encode  as createuserdeptcode,a.fullname, nvl(b.OrdinaryHid,0) as  OrdinaryHid, nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,a.sortcode  from
                                                    (select encode ,fullname,sortcode,departmentid,parentid from base_department  where nature='����' and  encode like '{0}%' ) a
                                                     left join (
                                                           select b.encode createuserdeptcode,sum(nvl(a.OrdinaryHid,0)) OrdinaryHid,sum(nvl(a.ImportanHid,0)) ImportanHid,sum(nvl(a.OrdinaryHid, 0) + nvl(a.ImportanHid, 0)) as total,b.fullname  from 
                                                            (
                                                                    select * from (
                                                                        select  count(1) as pnum ,  b.encode createuserdeptcode ,case when SUPERVISESTATE='0' then 'δ�ල'  when supervisestate='1' then '�Ѽල'  end as rankname  from bis_staffinfo a
                                                                        left join 
                                                                        (
                                                                           select encode ,fullname,sortcode from base_department  where nature='����' and  encode like '{0}%'
       
                                                                        ) b  on  substr(a.pteamcode,0,length(b.encode)) = b.encode 
                                                                        where a.pteamcode  like '{0}%'   and   {1} and tasklevel=2 group by  b.encode,SUPERVISESTATE
                                                                    ) pivot (sum(pnum) for rankname in ('δ�ල' as OrdinaryHid,'�Ѽල' as ImportanHid))
                                                            ) a 
                                                            left join base_department b on a.createuserdeptcode = b.encode group by b.encode,b.fullname
                                                    ) b on a.encode = b.createuserdeptcode  order by sortcode   ", sentity.sDeptCode, str);
                        }
                        else //�ǳ���
                        {
                            sql = string.Format(@"select a.createuserdeptcode,nvl(a.OrdinaryHid,0) OrdinaryHid,nvl(a.ImportanHid,0) ImportanHid,(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname,b.sortcode  from (
                                                select * from ( select  count(1) as pnum , pteamcode as createuserdeptcode,case when SUPERVISESTATE='0' then 'δ�ල'  when supervisestate='1' then '�Ѽල'  end as rankname  from bis_staffinfo where pteamcode  like '{0}%' 
                                                and  {1} and tasklevel=2  group by  pteamcode,supervisestate  
                                            ) pivot (sum(pnum) for rankname in ('δ�ල' as OrdinaryHid,'�Ѽල' as ImportanHid))) a 
                                            left join base_department b on a.createuserdeptcode = b.encode order by b.sortcode ", sentity.sDeptCode, str);
                        }
                    }
                    else if (sentity.sMark == 1) //�Ӽ��ල�Ա�ͼ��ѯ
                    {
                        DepartmentEntity dentity = Idepartmentservice.GetEntityByCode(sentity.sDeptCode);

                        if (dentity.Nature == "����")
                        {
                            sql = string.Format(@"select b.pteamcode,nvl(b.OrdinaryHid,0) OrdinaryHid,nvl(b.ImportanHid,0) ImportanHid,(nvl(b.OrdinaryHid,0) + nvl(b.ImportanHid,0)) as total,a.fullname,a.sortcode  from (
                                                   select encode , fullname,sortcode  from  base_department b where encode = '{0}' 
                                                 ) a 
                                                left join
                                                (
                                                  select * from (
                                                   select  count(1) as pnum , pteamcode,case when SUPERVISESTATE='0' then 'δ�ල'  when supervisestate='1' then '�Ѽල'  end as rankname  from bis_staffinfo where pteamcode  = '{0}' 
                                                   and  {1} and tasklevel=2   group by  pteamcode,SUPERVISESTATE  
                                                  ) pivot (sum(pnum) for rankname in ('δ�ල' as OrdinaryHid,'�Ѽල' as ImportanHid))
                                                ) b  on a.encode = b.pteamcode
                                                 order by a.sortcode  ", sentity.sDeptCode, str);
                        }
                        else
                        {
                            sql = string.Format(@"select b.pteamcode,nvl(b.OrdinaryHid,0) OrdinaryHid,nvl(b.ImportanHid,0) ImportanHid,(nvl(b.OrdinaryHid,0) + nvl(b.ImportanHid,0)) as total,a.fullname,a.sortcode  from (
                                                   select encode , fullname,sortcode  from  base_department b where encode like '{0}%' 
                                                 ) a 
                                                left join
                                                (
                                                  select * from (
                                                   select  count(1) as pnum , pteamcode,case when SUPERVISESTATE='0' then 'δ�ල'  when supervisestate='1' then '�Ѽල'  end as rankname  from bis_staffinfo where pteamcode  like '{0}%' 
                                                    and  {1} and tasklevel=2  group by  pteamcode,SUPERVISESTATE  
                                                  ) pivot (sum(pnum) for rankname in ('δ�ල' as OrdinaryHid,'�Ѽල' as ImportanHid))
                                                ) b  on a.encode = b.pteamcode
                                                 order by a.sortcode  ", sentity.sDeptCode, str);
                        }
                    }
                    else if (sentity.sMark == 2) //�ල�б��ѯ
                    {
                        //����
                        if (sentity.isCompany)
                        {
                            sql = string.Format(@"select a.encode  as createuserdeptcode,a.fullname, nvl(b.OrdinaryHid,0) as  OrdinaryHid, nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,a.sortcode,a.departmentid,'0'  parentid from
                                                    (select encode ,fullname,sortcode,departmentid,parentid from base_department  where nature='����'  and  encode like '{0}%' ) a
                                                     left join (
                                                           select b.encode createuserdeptcode,sum(nvl(a.OrdinaryHid,0)) OrdinaryHid,sum(nvl(a.ImportanHid,0)) ImportanHid,sum(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname  from 
                                                            (
                                                                    select * from (
                                                                        select  count(1) as pnum ,  b.encode createuserdeptcode ,case when SUPERVISESTATE='0' then 'δ�ල'  when supervisestate='1' then '�Ѽල'  end as rankname  from bis_staffinfo a
                                                                        left join 
                                                                        (
                                                                           select encode ,fullname,sortcode from base_department  where nature='����' and  encode like '{0}%'
                                                                        ) b  on  substr(a.pteamcode,0,length(b.encode)) = b.encode 
                                                                        where a.pteamcode  like '{0}%'   and   {1} and tasklevel=2 group by  b.encode,SUPERVISESTATE
                                                                    ) pivot (sum(pnum) for rankname in ('δ�ල' as OrdinaryHid,'�Ѽල' as ImportanHid))
                                                            ) a 
                                                            left join base_department b on a.createuserdeptcode = b.encode group by b.encode,b.fullname
                                                    ) b on a.encode = b.createuserdeptcode  
                                                    union
                                                    select a.encode  as createuserdeptcode,a.fullname, nvl(b.OrdinaryHid,0) as  OrdinaryHid, nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,a.sortcode,a.departmentid,a.parentid from
                                                    (select encode ,fullname,sortcode,departmentid,parentid from base_department  where nature!='����' and nature !='����'  and  encode like '{0}%' ) a
                                                     left join (
                                                       select b.encode createuserdeptcode,sum(nvl(a.OrdinaryHid,0)) OrdinaryHid,sum(nvl(a.ImportanHid,0)) ImportanHid,sum(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname  from 
                                                        (
                                                                select * from (
                                                                    select  count(1) as pnum ,  b.encode createuserdeptcode ,case when SUPERVISESTATE='0' then 'δ�ල'  when supervisestate='1' then '�Ѽල'  end as rankname  from bis_staffinfo a
                                                                    left join 
                                                                    (
                                                                       select encode ,fullname,sortcode from base_department  where nature!='����' and nature !='����'  and  encode like '{0}%'
                                                                    ) b  on  a.pteamcode = b.encode 
                                                                    where a.pteamcode  like '{0}%'  and  {1} and tasklevel=2 group by  b.encode,SUPERVISESTATE 
                                                                ) pivot (sum(pnum) for rankname in ('δ�ල' as OrdinaryHid,'�Ѽල' as ImportanHid))
                                                        ) a 
                                                        left join base_department b on a.createuserdeptcode = b.encode group by b.encode,b.fullname
                                                    ) b on a.encode = b.createuserdeptcode  
                                                    order by sortcode  ", sentity.sDeptCode, str);
                        }
                        else //�ǳ���
                        {
                            sql = string.Format(@"select a.createuserdeptcode,nvl(a.OrdinaryHid,0) OrdinaryHid,nvl(a.ImportanHid,0) ImportanHid,(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname,b.sortcode ,b.departmentid,'0' parentid  from (
                                                select * from ( select  count(1) as pnum , createuserdeptcode,case when SUPERVISESTATE='0' then 'δ�ල'  when supervisestate='1' then '�Ѽල'  end as rankname  from bis_staffinfo where createuserdeptcode  like '{0}%' 
                                                and  {1} and tasklevel=2  group by  createuserdeptcode,SUPERVISESTATE  
                                            ) pivot (sum(pnum) for rankname in ('δ�ල' as OrdinaryHid,'�Ѽල' as ImportanHid))) a 
                                            left join base_department b on a.createuserdeptcode = b.encode  
                                            order by b.encode ", sentity.sDeptCode, str);
                        }
                    }
                    #endregion
                    break;
                //��������б�
                case "2":
                    #region ��������б�
                    if (sentity.sMark == 0)
                    {
                        sql = string.Format(@"select a.encode  as createuserdeptcode,a.fullname, nvl(b.OrdinaryHid,0) as  OrdinaryHid, nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,a.sortcode  from
                                                    (select encode ,fullname,sortcode,departmentid,parentid from base_department  where nature='����' and  encode like '{0}%' ) a
                                                     left join (
                                                           select b.encode createuserdeptcode,sum(nvl(a.OrdinaryHid,0)) OrdinaryHid,sum(nvl(a.ImportanHid,0)) ImportanHid,sum(nvl(a.OrdinaryHid, 0) + nvl(a.ImportanHid, 0)) as total,b.fullname  from 
                                                            (
                                                                    select * from (
                                                                        select  count(1) as pnum ,  b.encode createuserdeptcode ,'����' as rankname  from bis_staffinfo a
                                                                        left join 
                                                                        (
                                                                           select encode ,fullname,sortcode from base_department  where nature='����' and  encode like '{0}%'
       
                                                                        ) b  on  substr(a.pteamcode,0,length(b.encode)) = b.encode 
                                                                        where a.pteamcode  like '{0}%'   and   {1} and tasklevel=1 and a.dataissubmit=1  group by  b.encode,SUPERVISESTATE
                                                                        union 
                                                                        select  count(1) as pnum ,  b.encode createuserdeptcode ,'�Ѽ��' as rankname  from bis_taskurge c
                                                                        left join bis_staffinfo a on c.STAFFID=a.id 
                                                                        left join 
                                                                        (
                                                                           select encode ,fullname,sortcode from base_department  where nature='����' and  encode like '{0}%'
       
                                                                        ) b  on  substr(a.pteamcode,0,length(b.encode)) = b.encode 
                                                                        where a.pteamcode  like '{0}%'   and   {1}  and tasklevel=1 and a.dataissubmit=1 and c.dataissubmit=1  group by  b.encode,SUPERVISESTATE
                                                                        
                                                                    ) pivot (sum(pnum) for rankname in ('����' as OrdinaryHid,'�Ѽ��' as ImportanHid))
                                                            ) a 
                                                            left join base_department b on a.createuserdeptcode = b.encode group by b.encode,b.fullname
                                                    ) b on a.encode = b.createuserdeptcode  order by sortcode   ", sentity.sDeptCode, str);
                    }
                    #endregion
                    break;
                case "3":
                    #region �ֻ�����վ�ල�б�
                    if (sentity.sMark == 0) //����б�
                    {
                        sql = string.Format(@"select a.encode  as createuserdeptcode,a.fullname, nvl(b.OrdinaryHid,0) as  OrdinaryHid, nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,a.sortcode  from
                                                    (select encode ,fullname,sortcode,departmentid,parentid from base_department  where nature='����' and  encode like '{0}%' ) a
                                                     left join (
                                                           select b.encode createuserdeptcode,sum(nvl(a.OrdinaryHid,0)) OrdinaryHid,sum(nvl(a.ImportanHid,0)) ImportanHid,sum(nvl(a.OrdinaryHid, 0) + nvl(a.ImportanHid, 0)) as total,b.fullname  from 
                                                            (
                                                                    select * from (
                                                                        select  count(1) as pnum ,  b.encode createuserdeptcode ,'����' as rankname  from bis_staffinfo a
                                                                        left join 
                                                                        (
                                                                           select encode ,fullname,sortcode from base_department  where nature='����' and  encode like '{0}%'
       
                                                                        ) b  on  substr(a.pteamcode,0,length(b.encode)) = b.encode 
                                                                        where a.pteamcode  like '{0}%'   and   {1} and tasklevel=1 and a.dataissubmit=1 group by  b.encode,SUPERVISESTATE
                                                                        union 
                                                                        select  count(1) as pnum ,  b.encode createuserdeptcode ,'�Ѽ��' as rankname  from bis_taskurge c
                                                                        left join bis_staffinfo a on c.STAFFID=a.id 
                                                                        left join 
                                                                        (
                                                                           select encode ,fullname,sortcode from base_department  where nature='����' and  encode like '{0}%'
       
                                                                        ) b  on  substr(a.pteamcode,0,length(b.encode)) = b.encode 
                                                                        where a.pteamcode  like '{0}%'   and   {1}  and tasklevel=1 and a.dataissubmit=1 and c.dataissubmit=1  group by  b.encode,SUPERVISESTATE
                                                                        
                                                                    ) pivot (sum(pnum) for rankname in ('����' as OrdinaryHid,'�Ѽ��' as ImportanHid))
                                                            ) a 
                                                            left join base_department b on a.createuserdeptcode = b.encode group by b.encode,b.fullname
                                                    ) b on a.encode = b.createuserdeptcode  order by sortcode   ", sentity.sDeptCode, str);
                    }
                    else //�ල�б�
                    {
                        DepartmentEntity dentity = Idepartmentservice.GetEntityByCode(sentity.sDeptCode);
                        //����
                        if (dentity.Nature=="����")
                        {
                            sql = string.Format(@"select a.encode  as createuserdeptcode,a.fullname, nvl(b.OrdinaryHid,0) as  OrdinaryHid, nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,a.sortcode,a.departmentid,'0'  parentid from
                                                    (select encode ,fullname,sortcode,departmentid,parentid from base_department  where nature='����'  and  encode like '{0}%' ) a
                                                     left join (
                                                           select b.encode createuserdeptcode,sum(nvl(a.OrdinaryHid,0)) OrdinaryHid,sum(nvl(a.ImportanHid,0)) ImportanHid,sum(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname  from 
                                                            (
                                                                    select * from (
                                                                        select  count(1) as pnum ,  b.encode createuserdeptcode ,case when SUPERVISESTATE='0' then 'δ�ල'  when supervisestate='1' then '�Ѽල'  end as rankname  from bis_staffinfo a
                                                                        left join 
                                                                        (
                                                                           select encode ,fullname,sortcode from base_department  where nature='����' and  encode like '{0}%'
                                                                        ) b  on  substr(a.pteamcode,0,length(b.encode)) = b.encode 
                                                                        where a.pteamcode  like '{0}%'   and   {1} and tasklevel=2 group by  b.encode,SUPERVISESTATE
                                                                    ) pivot (sum(pnum) for rankname in ('δ�ල' as OrdinaryHid,'�Ѽල' as ImportanHid))
                                                            ) a 
                                                            left join base_department b on a.createuserdeptcode = b.encode group by b.encode,b.fullname
                                                    ) b on a.encode = b.createuserdeptcode  
                                                    order by sortcode  ", sentity.sDeptCode, str);
                        }
                        else //�ǳ���
                        {
                            sql = string.Format(@"select a.encode  as createuserdeptcode,a.fullname, nvl(b.OrdinaryHid,0) as  OrdinaryHid, nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,a.sortcode,a.departmentid,a.parentid from
                                                    (select encode ,fullname,sortcode,departmentid,parentid from base_department  where  nature !='����'  and  encode like '{0}%' ) a
                                                     left join (
                                                       select b.encode createuserdeptcode,sum(nvl(a.OrdinaryHid,0)) OrdinaryHid,sum(nvl(a.ImportanHid,0)) ImportanHid,sum(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname  from 
                                                        (
                                                                select * from (
                                                                    select  count(1) as pnum ,  b.encode createuserdeptcode ,case when SUPERVISESTATE='0' then 'δ�ල'  when supervisestate='1' then '�Ѽල'  end as rankname  from bis_staffinfo a
                                                                    left join 
                                                                    (
                                                                       select encode ,fullname,sortcode from base_department  where  nature !='����'  and  encode like '{0}%'
                                                                    ) b  on  a.pteamcode = b.encode 
                                                                    where a.pteamcode  like '{0}%'  and  {1} and tasklevel=2 group by  b.encode,SUPERVISESTATE 
                                                                ) pivot (sum(pnum) for rankname in ('δ�ල' as OrdinaryHid,'�Ѽල' as ImportanHid))
                                                        ) a 
                                                        left join base_department b on a.createuserdeptcode = b.encode group by b.encode,b.fullname
                                                    ) b on a.encode = b.createuserdeptcode  
                                                    order by sortcode  ", sentity.sDeptCode, str);
                        }
                    }
                    #endregion
                    break;
            }

            if (!sql.IsEmpty())
            {
                var dt = this.BaseRepository().FindTable(sql);

                return dt;
            }
            else
            {
                return new DataTable();
            }
        }
        #endregion

        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                db.Delete<TaskShareEntity>(keyValue);
                db.Delete<SuperviseWorkInfoEntity>(t => t.TaskShareId.Equals(keyValue));
                var list = db.FindList<TeamsInfoEntity>(t => t.TaskShareId.Equals(keyValue));
                foreach (var item in list)
                {
                    db.Delete<TeamsWorkEntity>(t => t.TeamTaskId.Equals(item.Id));
                }
                db.Delete<TeamsInfoEntity>(t => t.TaskShareId.Equals(keyValue));
                var stafflist = db.FindList<StaffInfoEntity>(t => t.TaskShareId.Equals(keyValue));
                foreach (var item in stafflist)
                {
                    db.Delete<HTBaseInfoEntity>(t => t.RELEVANCEID.Equals(item.Id));
                    db.Delete<LllegalRegisterEntity>(t => t.RESEVERONE.Equals(item.Id));
                }
                db.Delete<StaffInfoEntity>(t => t.TaskShareId.Equals(keyValue));
                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public List<PushMessageData> SaveForm(string keyValue, TaskShareEntity entity)
        {
            var res = DbFactory.Base().BeginTrans();
            List<PushMessageData> listmessage = new List<PushMessageData>();
            //var flag = true;
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var curdeptid = user.DeptId;
                var curdeptcode = user.DeptCode;
                var curdeptname = user.DeptName;
                Repository<TaskShareEntity> rep = new Repository<TaskShareEntity>(DbFactory.Base());
                Repository<StaffInfoEntity> sep = new Repository<StaffInfoEntity>(DbFactory.Base());
                Repository<DepartmentEntity> dep = new Repository<DepartmentEntity>(DbFactory.Base());
                TaskShareEntity share = rep.FindEntity(keyValue);
                List<SuperviseWorkInfoEntity> workinfo = entity.WorkSpecs;//��ҵ��Ϣ
                List<TeamsInfoEntity> teaminfo = entity.TeamSpec;//������Ϣ
                List<StaffInfoEntity> staffinfo = entity.StaffSpec;//��Ա��Ϣ
                entity.Id = keyValue;
                if (share == null)
                {
                    entity.FlowDeptName = curdeptname;
                    if (entity.TaskType == "1")
                    {
                        entity.FlowStep = "1";
                    }
                    if (entity.TaskType == "2")
                    {
                        entity.FlowStep = "2";
                    }
                }
                else
                {
                    entity.FlowDeptName = share.FlowDeptName;
                }
                if (entity.FlowStep == "" || entity.FlowStep == "0")
                {
                    if (entity.IsSubmit == "1")//�������ŷ�����
                    {
                        entity.FlowDept = entity.SuperviseDeptId;
                        entity.FlowDeptName = curdeptname + "��" + entity.SuperviseDeptName;
                        entity.FlowStep = "1";
                        string strrole = new DataItemDetailService().GetItemValue(user.OrganizeId, "deptsuperviserole");
                        if (!string.IsNullOrEmpty(strrole))
                        {
                            entity.FlowRoleName = strrole;

                            PushMessageData messagedata = new PushMessageData();
                            //������Ϣ���з���Ȩ�޵���
                            messagedata.UserDept = entity.FlowDept;
                            messagedata.UserRole = entity.FlowRoleName;
                            messagedata.SendCode = "ZY017";
                            messagedata.EntityId = entity.Id;
                            listmessage.Add(messagedata);
                        }
                    }
                    else
                    {
                        entity.FlowStep = "0";
                    }
                    if (!string.IsNullOrEmpty(keyValue))
                    {
                        entity.Modify(keyValue);
                        entity.WorkSpecs = null;
                        entity.TeamSpec = null;
                        entity.StaffSpec = null;
                        entity.DelIds = null;
                        res.Update(entity);
                    }
                    else
                    {
                        entity.Create();
                        res.Insert(entity);
                    }
                    //��ӻ������ҵ��Ϣ ��ɾ�������
                    res.Delete<SuperviseWorkInfoEntity>(t => t.TaskShareId == entity.Id);
                    foreach (var wspec in workinfo)
                    {
                        wspec.HandType1 = wspec.HandType;
                        wspec.WorkInfoTypeId1 = wspec.WorkInfoTypeId;
                        wspec.WorkInfoType1 = wspec.WorkInfoType;
                        wspec.TaskShareId = entity.Id;
                        wspec.Create();
                        res.Insert(wspec);
                    }
                }
                else if (entity.FlowStep == "1")//���ŷ�����
                {
                    if (entity.IsSubmit == "1")
                    {
                        string teamids = string.Join(",", teaminfo.Select(x => x.TeamId).Distinct());
                        string teamnames = string.Join(",", teaminfo.Select(x => x.TeamName).Distinct());
                        entity.FlowDept = teamids;
                        entity.FlowDeptName = string.IsNullOrEmpty(entity.FlowDeptName) ? curdeptname + "��" + teamnames : entity.FlowDeptName + "��" + teamnames;
                        entity.FlowStep = "2";
                        string strrole = new DataItemDetailService().GetItemValue(user.OrganizeId, "teamrole");
                        if (!string.IsNullOrEmpty(strrole))
                        {
                            entity.FlowRoleName = strrole;

                            PushMessageData messagedata = new PushMessageData();
                            //������Ϣ���з���Ȩ�޵���
                            messagedata.UserDept = entity.FlowDept;
                            messagedata.UserRole = entity.FlowRoleName;
                            messagedata.SendCode = "ZY017";
                            messagedata.EntityId = entity.Id;
                            listmessage.Add(messagedata);
                        }
                    }
                    if (share == null)
                    {
                        if (entity.IsSubmit != "1")
                        {
                            entity.FlowStep = "1";
                        }
                        entity.Create();
                        res.Insert(entity);
                    }
                    else
                    {
                        entity.Modify(keyValue);
                        entity.WorkSpecs = null;
                        entity.TeamSpec = null;
                        entity.StaffSpec = null;
                        entity.DelIds = null;
                        res.Update(entity);
                    }
                    //��ӻ������ҵ��Ϣ ��ɾ�������
                    res.Delete<SuperviseWorkInfoEntity>(t => t.TaskShareId == entity.Id);
                    foreach (var wspec in workinfo)
                    {
                        wspec.HandType1 = wspec.HandType;
                        wspec.WorkInfoTypeId1 = wspec.WorkInfoTypeId;
                        wspec.WorkInfoType1 = wspec.WorkInfoType;
                        wspec.TaskShareId = entity.Id;
                        wspec.Create();
                        res.Insert(wspec);
                    }
                    //ɾ�����������Ϣ
                    res.Delete<TeamsInfoEntity>(t => t.TaskShareId == entity.Id);
                    foreach (var tspec in teaminfo)
                    {
                        tspec.TaskShareId = entity.Id;
                        tspec.Id = Guid.NewGuid().ToString();
                        tspec.DataIsSubmit = entity.IsSubmit;
                        tspec.Create();
                        res.Insert(tspec);
                        string[] arr = tspec.WorkInfoId.Split(',');
                        res.Delete<TeamsWorkEntity>(t => t.TeamTaskId == tspec.Id);
                        for (int j = 0; j < arr.Length; j++)
                        {
                            TeamsWorkEntity teamwork = new TeamsWorkEntity();
                            teamwork.WrokId = arr[j];
                            teamwork.TeamTaskId = tspec.Id;
                            teamwork.Create();
                            res.Insert(teamwork);
                        }
                    }
                }
                else if (entity.FlowStep == "2")//��Ա������
                {
                    string delids = entity.DelIds;
                    if (entity.TaskType == "2")//��Ա����
                    {
                        if (entity.IsSubmit == "1")
                        {
                            entity.FlowDept = "";
                            entity.FlowStep = "3";
                        }
                    }
                    if (share == null)
                    {
                        entity.DeptId = curdeptid;
                        entity.DeptName = curdeptname;
                        entity.DeptCode = curdeptcode;
                        entity.SuperviseDeptName = curdeptname;
                        entity.SuperviseDeptId = curdeptid;
                        entity.SuperviseDeptCode = curdeptcode;
                        if (entity.IsSubmit != "1")
                        {
                            //entity.FlowDept = curdeptid;
                            entity.FlowDeptName = curdeptname;
                            entity.FlowStep = "2";
                        }
                        entity.Create();
                        res.Insert(entity);
                    }
                    else
                    {
                        entity.Modify(keyValue);
                        entity.WorkSpecs = null;
                        entity.TeamSpec = null;
                        entity.StaffSpec = null;
                        entity.DelIds = null;
                        res.Update(entity);
                    }
                    if (entity.TaskType == "2")
                    {
                        //��ӻ������ҵ��Ϣ ��ɾ�������
                        res.Delete<SuperviseWorkInfoEntity>(t => t.TaskShareId == entity.Id);
                        foreach (var wspec in workinfo)
                        {
                            wspec.HandType1 = wspec.HandType;
                            wspec.WorkInfoTypeId1 = wspec.WorkInfoTypeId;
                            wspec.WorkInfoType1 = wspec.WorkInfoType;
                            wspec.TaskShareId = entity.Id;
                            wspec.Create();
                            res.Insert(wspec);
                        }
                    }
                    //var isstartdelids = "";//���Ͱ����ն�delids
                    //List<object> datas = new List<object>();
                    if (!string.IsNullOrEmpty(delids))
                    {
                        string[] arr = delids.Split(',');
                        for (int i = 0; i < arr.Length; i++)
                        {
                            //var mstaff = sep.FindEntity(arr[i]);
                            //if (!string.IsNullOrEmpty(mstaff.IsSynchronization))
                            //{
                            //    if (mstaff.IsSynchronization == "1")
                            //    {
                            //        isstartdelids += arr[i] + ",";
                            //    }
                            //}
                            res.Delete<StaffInfoEntity>(arr[i]);
                        }
                    }
                    //if (!string.IsNullOrEmpty(isstartdelids))
                    //{
                    //    isstartdelids = isstartdelids.TrimEnd(',');
                    //}
                    int count = 0;
                    string userid = "";
                    foreach (var wtspec in staffinfo)
                    {
                        if (wtspec.DataIsSubmit != "1")
                        {
                            if (entity.IsSubmit == "1")
                            {
                                if (!string.IsNullOrEmpty(wtspec.TaskUserId))
                                {
                                    wtspec.DataIsSubmit = "1";
                                }
                                wtspec.SuperviseState = "0";
                            }
                            var muchstaff = sep.FindEntity(wtspec.Id);
                            if (muchstaff != null)
                            {
                                wtspec.Modify(wtspec.Id);
                                res.Update(wtspec);
                                //if (wtspec.IsSynchronization == "1" && !string.IsNullOrEmpty(wtspec.TaskUserId))//���µ�����
                                //{
                                //    var tempdata = new
                                //    {
                                //        DataIsSubmit = wtspec.DataIsSubmit,//�����Ƿ��ύ
                                //        StartTime = wtspec.PStartTime,//��վ��ʼʱ��
                                //        EndTime = wtspec.PEndTime,//��վ����ʱ��
                                //        TaskUserId = wtspec.TaskUserId,//��ҵ����
                                //        TaskUserName = wtspec.TaskUserName,//��ҵ��λ
                                //        RecId = wtspec.Id,//����id
                                //    };
                                //    datas.Add(tempdata);
                                //}
                            }
                            else
                            {
                                wtspec.TaskLevel = "1";
                                wtspec.TaskShareId = entity.Id;
                                wtspec.SumTimeStr = 0;
                                wtspec.Create();
                                res.Insert(wtspec);
                            }
                            if (entity.IsSubmit == "1")
                            {
                                if (!string.IsNullOrEmpty(wtspec.TaskUserId))
                                {
                                    count++;
                                    string[] arrid = wtspec.TaskUserId.Split(',');
                                    string[] arrname = wtspec.TaskUserName.Split(',');
                                    for (int i = 0; i < arrid.Length; i++)
                                    {
                                        if (!userid.Contains(arrid[i]))
                                        {
                                            userid += userid + ",";
                                        }
                                        StaffInfoEntity staff = new StaffInfoEntity();
                                        staff.PTeamName = wtspec.PTeamName;
                                        staff.PTeamCode = wtspec.PTeamCode;
                                        staff.PTeamId = wtspec.PTeamId;
                                        staff.TaskUserId = arrid[i];
                                        staff.TaskUserName = arrname[i];
                                        staff.PStartTime = wtspec.PStartTime;
                                        staff.PEndTime = wtspec.PEndTime;
                                        staff.WorkInfoId = wtspec.WorkInfoId;
                                        staff.WorkInfoName = wtspec.WorkInfoName;
                                        staff.DataIsSubmit = "0";
                                        staff.TaskShareId = entity.Id;
                                        staff.SuperviseState = "0";
                                        staff.TaskLevel = "2";
                                        staff.StaffId = wtspec.Id;
                                        staff.SumTimeStr = 0;
                                        staff.SpecialtyType = wtspec.SpecialtyType;
                                        staff.Create();
                                        res.Insert(staff);
                                    }
                                }
                            }
                        }
                    }
                    if (count > 0)
                    {
                        PushMessageData messagedata = new PushMessageData();
                        //������Ϣ���������վ�ල
                        messagedata.SendCode = "ZY015";
                        messagedata.EntityId = entity.Id;
                        listmessage.Add(messagedata);

                        PushMessageData message = new PushMessageData();
                        if (!string.IsNullOrEmpty(userid))
                        {
                            userid = userid.TrimEnd(',');
                        }
                        //������Ϣ����ִ����վ�ල
                        message.UserId = userid;
                        message.SendCode = "ZY016";
                        message.EntityId = entity.Id;
                        listmessage.Add(message);
                    }
                    //var str = new DataItemDetailService().GetItemValue("�Ƿ�������ʱ����");
                    //if (str == "1" && entity.TaskType != "2")//������ʱ
                    //{
                    //    flag = false;
                    //    WebClient wc = new WebClient();
                    //    wc.Credentials = CredentialCache.DefaultCredentials;
                    //    //��������web api����ȡ����ֵ��Ĭ��Ϊpost��ʽ
                    //    var data = new
                    //    {
                    //        List = datas,
                    //        DelIds = delids
                    //    };
                    //    System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                    //    nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(data));
                    //    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                    //    System.IO.File.AppendAllText(new DataItemDetailService().GetItemValue("imgPath") + "/logs/" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "��ͬ���ɹ�,����Ϊ:" + Newtonsoft.Json.JsonConvert.SerializeObject(datas) + "\r\n");
                    //    wc.UploadValuesAsync(new Uri(new DataItemDetailService().GetItemValue("bzurl") + ""), nc);
                    //}
                }
                res.Commit();
                listmessage.ForEach(t => t.Success = 1);
            }
            catch (Exception ex)
            {
                listmessage.ForEach(t => t.Success = 0);
                //if (!flag)
                //{
                //    //��ͬ�����д����־�ļ�
                //    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                //    System.IO.File.AppendAllText(new DataItemDetailService().GetItemValue("imgPath") + "/logs/" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "������ʧ��" + ",�쳣��Ϣ��" + ex.Message + "\r\n");
                //}
                res.Rollback();
                throw ex;
            }
            return listmessage;
        }


        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveOnlyShare(string keyValue, TaskShareEntity entity)
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
