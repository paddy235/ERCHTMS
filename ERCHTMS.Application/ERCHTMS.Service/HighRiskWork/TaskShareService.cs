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
    /// 描 述：任务分配表
    /// </summary>
    public class TaskShareService : RepositoryFactory<TaskShareEntity>, TaskShareIService
    {
        private IDepartmentService Idepartmentservice = new DepartmentService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TaskShareEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TaskShareEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取分配任务列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetDataTable(Pagination page, string queryJson, string authType)
        {
            DatabaseType dataTye = DatabaseType.Oracle;
            #region 查表
            page.p_kid = "a.id";
            page.p_fields = @"a.tasktype,a.supervisedeptname,a.supervisedeptcode,a.createusername,a.createdate,a.flowstep,a.createuserid,a.issubmit,a.supervisedeptid,a.flowrolename,a.flowdept,a.flowdeptname,b.fullname";
            page.p_tablename = @"bis_taskshare a left join base_department b on  a.createuserdeptcode=b.encode";
            #endregion
            #region 数据权限
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
                            if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
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
            #region  筛选条件
            var queryParam = queryJson.ToJObject();
            //旁站监督单位
            if (!queryParam["SuperviseDeptCode"].IsEmpty())
            {
                page.conditionJson += string.Format(" and SuperviseDeptCode='{0}'", queryParam["SuperviseDeptCode"].ToString());
            }
            //创建单位
            if (!queryParam["CreateUserDeptCode"].IsEmpty())
            {
                page.conditionJson += string.Format(" and a.CreateUserDeptCode='{0}'", queryParam["CreateUserDeptCode"].ToString());
            }
            //待分配
            if (!queryParam["searchtype"].IsEmpty())
            {
                string[] roles = user.RoleName.Split(',');
                string roleWhere = "";
                foreach (var r in roles)
                {
                    roleWhere += string.Format("or a.flowrolename like '%{0}%'", r);
                }
                roleWhere = roleWhere.Substring(2);
                //当前有审核权限的部门及角色，才可查看
                page.conditionJson += string.Format("  and a.flowdept like '%{0}%' and ({1})", user.DeptId, roleWhere);
            }
            //排除其他人保存数据
            page.conditionJson += string.Format("  and a.id  not in(select id from bis_taskshare where issubmit='0' and  createuserid!='{0}' and flowdept is null)", user.UserId);
            #endregion

            return this.BaseRepository().FindTableByProcPager(page, dataTye);
        }

        #region 统计
        /// <summary>
        /// 旁站监督统计
        /// </summary>
        /// <param name="sentity"></param>
        /// <returns></returns>
        public DataTable QueryStatisticsByAction(StatisticsEntity sentity)
        {
            string sql = string.Empty;

            string str = " 1=1";


            Operator cuUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            DepartmentEntity dept = Idepartmentservice.GetEntityByCode(sentity.sDeptCode);

            if (dept.Nature == "厂级")
            {
                sentity.isCompany = true;
            }
            else
            {
                sentity.isCompany = false;
            }
            
            //起始日期
            if (!sentity.startDate.IsEmpty())
            {
                str += string.Format(" and pstarttime>=to_date('{0}','yyyy-mm-dd')", sentity.startDate);
            }
            //截止日期
            if (!sentity.endDate.IsEmpty())
            {
                str += string.Format(" and pendtime<=to_date('{0}','yyyy-mm-dd')", sentity.endDate);
            }
            switch (sentity.sAction)
            {

                //监督数量列表
                case "1":
                    #region 监督数量列表
                    if (sentity.sMark == 0) //父级监督对比图查询
                    {
                        //厂级
                        if (sentity.isCompany)
                        {
                            sql = string.Format(@"select a.encode  as createuserdeptcode,a.fullname, nvl(b.OrdinaryHid,0) as  OrdinaryHid, nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,a.sortcode  from
                                                    (select encode ,fullname,sortcode,departmentid,parentid from base_department  where nature='部门' and  encode like '{0}%' ) a
                                                     left join (
                                                           select b.encode createuserdeptcode,sum(nvl(a.OrdinaryHid,0)) OrdinaryHid,sum(nvl(a.ImportanHid,0)) ImportanHid,sum(nvl(a.OrdinaryHid, 0) + nvl(a.ImportanHid, 0)) as total,b.fullname  from 
                                                            (
                                                                    select * from (
                                                                        select  count(1) as pnum ,  b.encode createuserdeptcode ,case when SUPERVISESTATE='0' then '未监督'  when supervisestate='1' then '已监督'  end as rankname  from bis_staffinfo a
                                                                        left join 
                                                                        (
                                                                           select encode ,fullname,sortcode from base_department  where nature='部门' and  encode like '{0}%'
       
                                                                        ) b  on  substr(a.pteamcode,0,length(b.encode)) = b.encode 
                                                                        where a.pteamcode  like '{0}%'   and   {1} and tasklevel=2 group by  b.encode,SUPERVISESTATE
                                                                    ) pivot (sum(pnum) for rankname in ('未监督' as OrdinaryHid,'已监督' as ImportanHid))
                                                            ) a 
                                                            left join base_department b on a.createuserdeptcode = b.encode group by b.encode,b.fullname
                                                    ) b on a.encode = b.createuserdeptcode  order by sortcode   ", sentity.sDeptCode, str);
                        }
                        else //非厂级
                        {
                            sql = string.Format(@"select a.createuserdeptcode,nvl(a.OrdinaryHid,0) OrdinaryHid,nvl(a.ImportanHid,0) ImportanHid,(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname,b.sortcode  from (
                                                select * from ( select  count(1) as pnum , pteamcode as createuserdeptcode,case when SUPERVISESTATE='0' then '未监督'  when supervisestate='1' then '已监督'  end as rankname  from bis_staffinfo where pteamcode  like '{0}%' 
                                                and  {1} and tasklevel=2  group by  pteamcode,supervisestate  
                                            ) pivot (sum(pnum) for rankname in ('未监督' as OrdinaryHid,'已监督' as ImportanHid))) a 
                                            left join base_department b on a.createuserdeptcode = b.encode order by b.sortcode ", sentity.sDeptCode, str);
                        }
                    }
                    else if (sentity.sMark == 1) //子级监督对比图查询
                    {
                        DepartmentEntity dentity = Idepartmentservice.GetEntityByCode(sentity.sDeptCode);

                        if (dentity.Nature == "厂级")
                        {
                            sql = string.Format(@"select b.pteamcode,nvl(b.OrdinaryHid,0) OrdinaryHid,nvl(b.ImportanHid,0) ImportanHid,(nvl(b.OrdinaryHid,0) + nvl(b.ImportanHid,0)) as total,a.fullname,a.sortcode  from (
                                                   select encode , fullname,sortcode  from  base_department b where encode = '{0}' 
                                                 ) a 
                                                left join
                                                (
                                                  select * from (
                                                   select  count(1) as pnum , pteamcode,case when SUPERVISESTATE='0' then '未监督'  when supervisestate='1' then '已监督'  end as rankname  from bis_staffinfo where pteamcode  = '{0}' 
                                                   and  {1} and tasklevel=2   group by  pteamcode,SUPERVISESTATE  
                                                  ) pivot (sum(pnum) for rankname in ('未监督' as OrdinaryHid,'已监督' as ImportanHid))
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
                                                   select  count(1) as pnum , pteamcode,case when SUPERVISESTATE='0' then '未监督'  when supervisestate='1' then '已监督'  end as rankname  from bis_staffinfo where pteamcode  like '{0}%' 
                                                    and  {1} and tasklevel=2  group by  pteamcode,SUPERVISESTATE  
                                                  ) pivot (sum(pnum) for rankname in ('未监督' as OrdinaryHid,'已监督' as ImportanHid))
                                                ) b  on a.encode = b.pteamcode
                                                 order by a.sortcode  ", sentity.sDeptCode, str);
                        }
                    }
                    else if (sentity.sMark == 2) //监督列表查询
                    {
                        //厂级
                        if (sentity.isCompany)
                        {
                            sql = string.Format(@"select a.encode  as createuserdeptcode,a.fullname, nvl(b.OrdinaryHid,0) as  OrdinaryHid, nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,a.sortcode,a.departmentid,'0'  parentid from
                                                    (select encode ,fullname,sortcode,departmentid,parentid from base_department  where nature='部门'  and  encode like '{0}%' ) a
                                                     left join (
                                                           select b.encode createuserdeptcode,sum(nvl(a.OrdinaryHid,0)) OrdinaryHid,sum(nvl(a.ImportanHid,0)) ImportanHid,sum(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname  from 
                                                            (
                                                                    select * from (
                                                                        select  count(1) as pnum ,  b.encode createuserdeptcode ,case when SUPERVISESTATE='0' then '未监督'  when supervisestate='1' then '已监督'  end as rankname  from bis_staffinfo a
                                                                        left join 
                                                                        (
                                                                           select encode ,fullname,sortcode from base_department  where nature='部门' and  encode like '{0}%'
                                                                        ) b  on  substr(a.pteamcode,0,length(b.encode)) = b.encode 
                                                                        where a.pteamcode  like '{0}%'   and   {1} and tasklevel=2 group by  b.encode,SUPERVISESTATE
                                                                    ) pivot (sum(pnum) for rankname in ('未监督' as OrdinaryHid,'已监督' as ImportanHid))
                                                            ) a 
                                                            left join base_department b on a.createuserdeptcode = b.encode group by b.encode,b.fullname
                                                    ) b on a.encode = b.createuserdeptcode  
                                                    union
                                                    select a.encode  as createuserdeptcode,a.fullname, nvl(b.OrdinaryHid,0) as  OrdinaryHid, nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,a.sortcode,a.departmentid,a.parentid from
                                                    (select encode ,fullname,sortcode,departmentid,parentid from base_department  where nature!='部门' and nature !='厂级'  and  encode like '{0}%' ) a
                                                     left join (
                                                       select b.encode createuserdeptcode,sum(nvl(a.OrdinaryHid,0)) OrdinaryHid,sum(nvl(a.ImportanHid,0)) ImportanHid,sum(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname  from 
                                                        (
                                                                select * from (
                                                                    select  count(1) as pnum ,  b.encode createuserdeptcode ,case when SUPERVISESTATE='0' then '未监督'  when supervisestate='1' then '已监督'  end as rankname  from bis_staffinfo a
                                                                    left join 
                                                                    (
                                                                       select encode ,fullname,sortcode from base_department  where nature!='部门' and nature !='厂级'  and  encode like '{0}%'
                                                                    ) b  on  a.pteamcode = b.encode 
                                                                    where a.pteamcode  like '{0}%'  and  {1} and tasklevel=2 group by  b.encode,SUPERVISESTATE 
                                                                ) pivot (sum(pnum) for rankname in ('未监督' as OrdinaryHid,'已监督' as ImportanHid))
                                                        ) a 
                                                        left join base_department b on a.createuserdeptcode = b.encode group by b.encode,b.fullname
                                                    ) b on a.encode = b.createuserdeptcode  
                                                    order by sortcode  ", sentity.sDeptCode, str);
                        }
                        else //非厂级
                        {
                            sql = string.Format(@"select a.createuserdeptcode,nvl(a.OrdinaryHid,0) OrdinaryHid,nvl(a.ImportanHid,0) ImportanHid,(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname,b.sortcode ,b.departmentid,'0' parentid  from (
                                                select * from ( select  count(1) as pnum , createuserdeptcode,case when SUPERVISESTATE='0' then '未监督'  when supervisestate='1' then '已监督'  end as rankname  from bis_staffinfo where createuserdeptcode  like '{0}%' 
                                                and  {1} and tasklevel=2  group by  createuserdeptcode,SUPERVISESTATE  
                                            ) pivot (sum(pnum) for rankname in ('未监督' as OrdinaryHid,'已监督' as ImportanHid))) a 
                                            left join base_department b on a.createuserdeptcode = b.encode  
                                            order by b.encode ", sentity.sDeptCode, str);
                        }
                    }
                    #endregion
                    break;
                //监管数量列表
                case "2":
                    #region 监管数量列表
                    if (sentity.sMark == 0)
                    {
                        sql = string.Format(@"select a.encode  as createuserdeptcode,a.fullname, nvl(b.OrdinaryHid,0) as  OrdinaryHid, nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,a.sortcode  from
                                                    (select encode ,fullname,sortcode,departmentid,parentid from base_department  where nature='部门' and  encode like '{0}%' ) a
                                                     left join (
                                                           select b.encode createuserdeptcode,sum(nvl(a.OrdinaryHid,0)) OrdinaryHid,sum(nvl(a.ImportanHid,0)) ImportanHid,sum(nvl(a.OrdinaryHid, 0) + nvl(a.ImportanHid, 0)) as total,b.fullname  from 
                                                            (
                                                                    select * from (
                                                                        select  count(1) as pnum ,  b.encode createuserdeptcode ,'需监管' as rankname  from bis_staffinfo a
                                                                        left join 
                                                                        (
                                                                           select encode ,fullname,sortcode from base_department  where nature='部门' and  encode like '{0}%'
       
                                                                        ) b  on  substr(a.pteamcode,0,length(b.encode)) = b.encode 
                                                                        where a.pteamcode  like '{0}%'   and   {1} and tasklevel=1 and a.dataissubmit=1  group by  b.encode,SUPERVISESTATE
                                                                        union 
                                                                        select  count(1) as pnum ,  b.encode createuserdeptcode ,'已监管' as rankname  from bis_taskurge c
                                                                        left join bis_staffinfo a on c.STAFFID=a.id 
                                                                        left join 
                                                                        (
                                                                           select encode ,fullname,sortcode from base_department  where nature='部门' and  encode like '{0}%'
       
                                                                        ) b  on  substr(a.pteamcode,0,length(b.encode)) = b.encode 
                                                                        where a.pteamcode  like '{0}%'   and   {1}  and tasklevel=1 and a.dataissubmit=1 and c.dataissubmit=1  group by  b.encode,SUPERVISESTATE
                                                                        
                                                                    ) pivot (sum(pnum) for rankname in ('需监管' as OrdinaryHid,'已监管' as ImportanHid))
                                                            ) a 
                                                            left join base_department b on a.createuserdeptcode = b.encode group by b.encode,b.fullname
                                                    ) b on a.encode = b.createuserdeptcode  order by sortcode   ", sentity.sDeptCode, str);
                    }
                    #endregion
                    break;
                case "3":
                    #region 手机端旁站监督列表
                    if (sentity.sMark == 0) //监管列表
                    {
                        sql = string.Format(@"select a.encode  as createuserdeptcode,a.fullname, nvl(b.OrdinaryHid,0) as  OrdinaryHid, nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,a.sortcode  from
                                                    (select encode ,fullname,sortcode,departmentid,parentid from base_department  where nature='部门' and  encode like '{0}%' ) a
                                                     left join (
                                                           select b.encode createuserdeptcode,sum(nvl(a.OrdinaryHid,0)) OrdinaryHid,sum(nvl(a.ImportanHid,0)) ImportanHid,sum(nvl(a.OrdinaryHid, 0) + nvl(a.ImportanHid, 0)) as total,b.fullname  from 
                                                            (
                                                                    select * from (
                                                                        select  count(1) as pnum ,  b.encode createuserdeptcode ,'需监管' as rankname  from bis_staffinfo a
                                                                        left join 
                                                                        (
                                                                           select encode ,fullname,sortcode from base_department  where nature='部门' and  encode like '{0}%'
       
                                                                        ) b  on  substr(a.pteamcode,0,length(b.encode)) = b.encode 
                                                                        where a.pteamcode  like '{0}%'   and   {1} and tasklevel=1 and a.dataissubmit=1 group by  b.encode,SUPERVISESTATE
                                                                        union 
                                                                        select  count(1) as pnum ,  b.encode createuserdeptcode ,'已监管' as rankname  from bis_taskurge c
                                                                        left join bis_staffinfo a on c.STAFFID=a.id 
                                                                        left join 
                                                                        (
                                                                           select encode ,fullname,sortcode from base_department  where nature='部门' and  encode like '{0}%'
       
                                                                        ) b  on  substr(a.pteamcode,0,length(b.encode)) = b.encode 
                                                                        where a.pteamcode  like '{0}%'   and   {1}  and tasklevel=1 and a.dataissubmit=1 and c.dataissubmit=1  group by  b.encode,SUPERVISESTATE
                                                                        
                                                                    ) pivot (sum(pnum) for rankname in ('需监管' as OrdinaryHid,'已监管' as ImportanHid))
                                                            ) a 
                                                            left join base_department b on a.createuserdeptcode = b.encode group by b.encode,b.fullname
                                                    ) b on a.encode = b.createuserdeptcode  order by sortcode   ", sentity.sDeptCode, str);
                    }
                    else //监督列表
                    {
                        DepartmentEntity dentity = Idepartmentservice.GetEntityByCode(sentity.sDeptCode);
                        //厂级
                        if (dentity.Nature=="厂级")
                        {
                            sql = string.Format(@"select a.encode  as createuserdeptcode,a.fullname, nvl(b.OrdinaryHid,0) as  OrdinaryHid, nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,a.sortcode,a.departmentid,'0'  parentid from
                                                    (select encode ,fullname,sortcode,departmentid,parentid from base_department  where nature='部门'  and  encode like '{0}%' ) a
                                                     left join (
                                                           select b.encode createuserdeptcode,sum(nvl(a.OrdinaryHid,0)) OrdinaryHid,sum(nvl(a.ImportanHid,0)) ImportanHid,sum(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname  from 
                                                            (
                                                                    select * from (
                                                                        select  count(1) as pnum ,  b.encode createuserdeptcode ,case when SUPERVISESTATE='0' then '未监督'  when supervisestate='1' then '已监督'  end as rankname  from bis_staffinfo a
                                                                        left join 
                                                                        (
                                                                           select encode ,fullname,sortcode from base_department  where nature='部门' and  encode like '{0}%'
                                                                        ) b  on  substr(a.pteamcode,0,length(b.encode)) = b.encode 
                                                                        where a.pteamcode  like '{0}%'   and   {1} and tasklevel=2 group by  b.encode,SUPERVISESTATE
                                                                    ) pivot (sum(pnum) for rankname in ('未监督' as OrdinaryHid,'已监督' as ImportanHid))
                                                            ) a 
                                                            left join base_department b on a.createuserdeptcode = b.encode group by b.encode,b.fullname
                                                    ) b on a.encode = b.createuserdeptcode  
                                                    order by sortcode  ", sentity.sDeptCode, str);
                        }
                        else //非厂级
                        {
                            sql = string.Format(@"select a.encode  as createuserdeptcode,a.fullname, nvl(b.OrdinaryHid,0) as  OrdinaryHid, nvl(b.ImportanHid,0) as ImportanHid,nvl(b.total,0) as total ,a.sortcode,a.departmentid,a.parentid from
                                                    (select encode ,fullname,sortcode,departmentid,parentid from base_department  where  nature !='厂级'  and  encode like '{0}%' ) a
                                                     left join (
                                                       select b.encode createuserdeptcode,sum(nvl(a.OrdinaryHid,0)) OrdinaryHid,sum(nvl(a.ImportanHid,0)) ImportanHid,sum(nvl(a.OrdinaryHid,0) + nvl(a.ImportanHid,0)) as total,b.fullname  from 
                                                        (
                                                                select * from (
                                                                    select  count(1) as pnum ,  b.encode createuserdeptcode ,case when SUPERVISESTATE='0' then '未监督'  when supervisestate='1' then '已监督'  end as rankname  from bis_staffinfo a
                                                                    left join 
                                                                    (
                                                                       select encode ,fullname,sortcode from base_department  where  nature !='厂级'  and  encode like '{0}%'
                                                                    ) b  on  a.pteamcode = b.encode 
                                                                    where a.pteamcode  like '{0}%'  and  {1} and tasklevel=2 group by  b.encode,SUPERVISESTATE 
                                                                ) pivot (sum(pnum) for rankname in ('未监督' as OrdinaryHid,'已监督' as ImportanHid))
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

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
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
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
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
                List<SuperviseWorkInfoEntity> workinfo = entity.WorkSpecs;//作业信息
                List<TeamsInfoEntity> teaminfo = entity.TeamSpec;//班组信息
                List<StaffInfoEntity> staffinfo = entity.StaffSpec;//人员信息
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
                    if (entity.IsSubmit == "1")//厂级部门分配中
                    {
                        entity.FlowDept = entity.SuperviseDeptId;
                        entity.FlowDeptName = curdeptname + "→" + entity.SuperviseDeptName;
                        entity.FlowStep = "1";
                        string strrole = new DataItemDetailService().GetItemValue(user.OrganizeId, "deptsuperviserole");
                        if (!string.IsNullOrEmpty(strrole))
                        {
                            entity.FlowRoleName = strrole;

                            PushMessageData messagedata = new PushMessageData();
                            //推送消息到有分配权限的人
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
                    //添加或更新作业信息 先删除再添加
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
                else if (entity.FlowStep == "1")//部门分配中
                {
                    if (entity.IsSubmit == "1")
                    {
                        string teamids = string.Join(",", teaminfo.Select(x => x.TeamId).Distinct());
                        string teamnames = string.Join(",", teaminfo.Select(x => x.TeamName).Distinct());
                        entity.FlowDept = teamids;
                        entity.FlowDeptName = string.IsNullOrEmpty(entity.FlowDeptName) ? curdeptname + "→" + teamnames : entity.FlowDeptName + "→" + teamnames;
                        entity.FlowStep = "2";
                        string strrole = new DataItemDetailService().GetItemValue(user.OrganizeId, "teamrole");
                        if (!string.IsNullOrEmpty(strrole))
                        {
                            entity.FlowRoleName = strrole;

                            PushMessageData messagedata = new PushMessageData();
                            //推送消息到有分配权限的人
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
                    //添加或更新作业信息 先删除再添加
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
                    //删除班组分配信息
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
                else if (entity.FlowStep == "2")//人员分配中
                {
                    string delids = entity.DelIds;
                    if (entity.TaskType == "2")//人员任务
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
                        //添加或更新作业信息 先删除再添加
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
                    //var isstartdelids = "";//推送班组终端delids
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
                                //if (wtspec.IsSynchronization == "1" && !string.IsNullOrEmpty(wtspec.TaskUserId))//更新到班组
                                //{
                                //    var tempdata = new
                                //    {
                                //        DataIsSubmit = wtspec.DataIsSubmit,//数据是否提交
                                //        StartTime = wtspec.PStartTime,//旁站开始时间
                                //        EndTime = wtspec.PEndTime,//旁站结束时间
                                //        TaskUserId = wtspec.TaskUserId,//作业内容
                                //        TaskUserName = wtspec.TaskUserName,//作业单位
                                //        RecId = wtspec.Id,//主键id
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
                        //推送消息到待监管旁站监督
                        messagedata.SendCode = "ZY015";
                        messagedata.EntityId = entity.Id;
                        listmessage.Add(messagedata);

                        PushMessageData message = new PushMessageData();
                        if (!string.IsNullOrEmpty(userid))
                        {
                            userid = userid.TrimEnd(',');
                        }
                        //推送消息到待执行旁站监督
                        message.UserId = userid;
                        message.SendCode = "ZY016";
                        message.EntityId = entity.Id;
                        listmessage.Add(message);
                    }
                    //var str = new DataItemDetailService().GetItemValue("是否启动定时服务");
                    //if (str == "1" && entity.TaskType != "2")//启动定时
                    //{
                    //    flag = false;
                    //    WebClient wc = new WebClient();
                    //    wc.Credentials = CredentialCache.DefaultCredentials;
                    //    //发送请求到web api并获取返回值，默认为post方式
                    //    var data = new
                    //    {
                    //        List = datas,
                    //        DelIds = delids
                    //    };
                    //    System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                    //    nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(data));
                    //    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                    //    System.IO.File.AppendAllText(new DataItemDetailService().GetItemValue("imgPath") + "/logs/" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步成功,数据为:" + Newtonsoft.Json.JsonConvert.SerializeObject(datas) + "\r\n");
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
                //    //将同步结果写入日志文件
                //    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                //    System.IO.File.AppendAllText(new DataItemDetailService().GetItemValue("imgPath") + "/logs/" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：数据失败" + ",异常信息：" + ex.Message + "\r\n");
                //}
                res.Rollback();
                throw ex;
            }
            return listmessage;
        }


        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
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
