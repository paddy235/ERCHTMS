using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using System.Data;
using BSFramework.Data;
using System.Text;
using System;
using ERCHTMS.Code;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.SystemManage;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包工程信息表
    /// </summary>
    public class OutsouringengineerService : RepositoryFactory<OutsouringengineerEntity>, OutsouringengineerIService
    {
        private DepartmentService DepartmentService = new DepartmentService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["name"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (e.engineername like'%{0}%' or b.fullname like'%{1}%') ", queryParam["name"].ToString(), queryParam["name"].ToString());
            }
            if (!queryParam["engineertype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and e.engineertype='{0}' ", queryParam["engineertype"].ToString());
            }
            if (!queryParam["engineerlevel"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and e.engineerlevel='{0}' ", queryParam["engineerlevel"].ToString());
            }
            if (!queryParam["OutProjectId"].IsEmpty())
            {
                var dept = new DepartmentService().GetEntity(queryParam["OutProjectId"].ToString());
                if (dept!=null)
                {
                    pagination.conditionJson += string.Format(@" and e.outprojectid=(select departmentid from (select t.departmentid from base_department t 
                                                                    where instr('{0}',encode)>0 
                                                                    and nature='承包商' order by encode asc )t where rownum=1) ",
                                                                                         dept.EnCode);
                
                }
                else
                {
                    pagination.conditionJson += string.Format(" and e.outprojectid='{0}' ", queryParam["OutProjectId"].ToString());
                }
            }
            //工程状态
            if (!queryParam["engineerstate"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and e.engineerstate='{0}' ", queryParam["engineerstate"].ToString());
            }
            //工程状态 多选
            if (!queryParam["engineerstatIn"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and e.engineerstate in({0}) ", queryParam["engineerstatIn"].ToString());
            }
            //厂级统计跳转
            if (!queryParam["sTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and e.createdate>=to_date('{0}','yyyy-mm-dd') ", queryParam["sTime"].ToString());
            }
            if (!queryParam["eTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and e.createdate<=to_date('{0}','yyyy-mm-dd') ", Convert.ToDateTime(queryParam["eTime"]).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //省级统计跳转使用--请勿删除
            if (!queryParam["fullName"].IsEmpty())
            {
                if (queryParam["fullName"].ToString() == "全部")
                {

                }
                else
                {
                    if (!queryParam["orgCode"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(" and e.createuserorgcode='{0}' ", queryParam["orgCode"].ToString());
                    }
                }
            }


            //隐患跳转-请勿删除
            if (!queryParam["orgid"].IsEmpty())
            {
                DataTable dt1 = this.BaseRepository().FindTable(string.Format("select encode from base_department where departmentid='{0}' ", queryParam["orgid"].ToString()));
                pagination.conditionJson += string.Format(" and e.createuserorgcode like'%{0}%'", dt1.Rows[0][0].ToString());
            }
            if (!queryParam["Time"].IsEmpty())
            {
                var startTime = DateTime.Parse(queryParam["Time"].ToString());
                var endTime = startTime.AddMonths(1).AddDays(-1);
                pagination.conditionJson += string.Format(" and ( to_char(e.planenddate,'yyyy-MM-dd') <='{0}' and e.planenddate is not null) and ( to_char(e.actualenddate ,'yyyy-MM-dd')>= '{1}' or  e.actualenddate  is null)", endTime.ToString("yyyy-MM-dd"), startTime.ToString("yyyy-MM-dd"));
            }
            if (!queryParam["IsDeptAdd"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and e.isdeptadd={0} ", Convert.ToInt32(queryParam["IsDeptAdd"].ToString()));
            }

            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            dt.Columns.Add("processState", typeof(string));
            dt.Columns.Add("SafeEvaNum", typeof(Int32));
            foreach (DataRow item in dt.Rows)
            {
                var slist = this.BaseRepository().FindList(string.Format("select * from EPG_SAFETYEVALUATE t where t.projectid='{0}'", item["id"].ToString())).ToList();
                item["SafeEvaNum"] = slist.Count;
                if (!string.IsNullOrWhiteSpace(item["EXAMSTATUS"].ToString()))
                {
                    item["processState"] = item["EXAMSTATUS"].ToString();
                    continue;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(item["pactstatus"].ToString()))
                        item["processState"] += item["pactstatus"].ToString() + "、";
                    if (!string.IsNullOrWhiteSpace(item["threetwostatus"].ToString()))
                        item["processState"] += item["threetwostatus"].ToString() + "、";
                    if (!string.IsNullOrWhiteSpace(item["compactstatus"].ToString()))
                        item["processState"] += item["compactstatus"].ToString() + "、";
                    if (!string.IsNullOrWhiteSpace(item["technicalstatus"].ToString()))
                        item["processState"] += item["technicalstatus"].ToString() + "、";
                    if (!string.IsNullOrWhiteSpace(item["equipmenttoolstatus"].ToString()))
                        item["processState"] += item["equipmenttoolstatus"].ToString() + "、";
                    if (!string.IsNullOrWhiteSpace(item["peoplestatus"].ToString()))
                        item["processState"] += item["peoplestatus"].ToString() + "、";

                    if (item["processState"].ToString().Length > 0)
                    {
                        item["processState"] = item["processState"].ToString().Substring(0, item["processState"].ToString().Length - 1);
                    }
                    if (item["processState"].ToString().Length == 0)
                    {
                        item["processState"] = "开工申请";
                    }
                }
                if (item["engineerstate"].ToString() == "在建")
                {
                    item["processState"] = "开工中";
                }
                if (item["engineerstate"].ToString() == "已完工")
                {
                    item["processState"] = "已完工";
                }
            }


            return dt;
            //return this.BaseRepository().FindList(pagination);
        }


        public DataTable GetIndexToList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["pageType"].IsEmpty())
            {
                switch (queryParam["pageType"].ToString())
                {
                    case "1"://保证金
                        pagination.conditionJson += string.Format(" and e.id not in (select distinct(m.projectid) from epg_safetyeamestmoney m)");
                        break;
                    case "2"://合同
                        pagination.conditionJson += string.Format(" and e.id not in (select distinct(m.projectid) from epg_compact m)");
                        break;
                    case "3"://协议
                        pagination.conditionJson += string.Format(" and e.id not in (select distinct(m.projectid) from epg_protocol m)");
                        break;
                    case "4"://安全技术交底
                        pagination.conditionJson += string.Format("  and e.id not in (select distinct(m.projectid) from epg_techdisclosure m) ");
                        break;
                    default:
                        break;
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);

        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OutsouringengineerEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OutsouringengineerEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OutsouringengineerEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 根据当前登陆人和配置信息查询工程信息
        /// </summary>
        /// <param name="currUser">当前用户</param>
        /// <param name="mode">001--单位资质 002--人员资质 003--合同 004--协议 005--安全技术交底 006--三措两案 007--工器具验收  008--入场许可 009--开工申请 010--保证金 </param>
        /// <param name="orgid">电厂ID</param>
        /// <returns></returns>
        public DataTable GetEngineerDataByCurrdeptId(Operator currUser, string mode = "", string orgid = "")
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select e.engineername engineername,
                                         e.id engineerid,e.outprojectid unitid,d.fullname unitname,d.encode,
                                            engineerletdept deptname,engineerletdeptid deptid, zr.encode letdeptcode , engineercode projectcode,
                                            e.engineerarea  areaid, ENGINEERAREANAME areaname,f.itemname projecttype,f.itemvalue projecttypecode,g.itemname projectlevel, g.itemvalue projectlevelcode,
                                            engineercontent projectcontent
                                    from epg_outsouringengineer e 
                                    left join epg_startappprocessstatus p on p.outengineerid=e.id
                                    left join base_department d on e.outprojectid=d.departmentid
                                    left join base_department zr on e.engineerletdeptid=zr.departmentid
                                    left join bis_district r on e.engineerarea=r.districtid
                                    left join base_dataitemdetail f on e.engineertype=f.itemvalue 
                                    left join base_dataitemdetail g on e.ENGINEERLEVEL=g.itemvalue
                                    where f.itemid=(select itemid from base_dataitem where itemcode='ProjectType')
                                    and g.itemid=(select itemid from base_dataitem where itemcode='ProjectLevel')");
            if (!currUser.IsSystem)
            {
                string allrangedept = new DataItemDetailService().GetItemValue("设备管理部", "SBDept");
                if (currUser.RoleName.Contains("省级用户"))
                {
                    string tWhere = " 1=1";
                    if (!orgid.IsEmpty())
                    {
                        tWhere += string.Format(" and d.organizeid ='{0}'", orgid);
                    }
                    strSql.AppendFormat(@" and e.createuserorgcode in (select encode from base_department d where d.deptcode like '{0}%' and d.nature='厂级' and d.description is null and {1})", currUser.NewDeptCode, tWhere);
                }
                else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户") || currUser.RoleName.Contains("公司管理员") || currUser.RoleName.Contains("公司领导") || currUser.DeptId== allrangedept)
                {
                    strSql.Append(string.Format(" and e.createuserorgcode like'%{0}%' ", currUser.OrganizeCode));
                }
                else if (currUser.RoleName.Contains("承包商级用户"))
                {
                    strSql.Append(string.Format(" and (e.outprojectid ='{0}' or e.SUPERVISORID='{0}')", currUser.DeptId));
                }
                else
                {
                    var deptentity = DepartmentService.GetEntity(currUser.DeptId);
                    while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
                    {
                        deptentity = DepartmentService.GetEntity(deptentity.ParentId);
                    }
                    strSql.Append(string.Format(" and e.engineerletdeptid in (select departmentid from base_department where encode like '{0}%') ", deptentity.EnCode));
                    //strSql.Append(string.Format(" and e.engineerletdeptid ='{0}'", currUser.DeptId));
                }
                strSql.Append(" and isdeptadd=1 ");
            }
            if (!string.IsNullOrEmpty(mode))
            {   
                if (mode == "009")
                {
                    strSql.Append(string.Format(" and e.id not in(select outengineerid from epg_startapplyfor）  "));
                }
                if (mode == "012")
                {
                    strSql.Append(string.Format(" and e.id not in(select projectid from epg_safetyevaluate）and e.engineerstate='002' "));
                }
                if (mode == "013") // 安全信用评价计划查询在建和已完成 刘畅 2020/10/14
                {
                    strSql.Append(string.Format(" and e.engineerstate in('002','003') "));
                }
                if (mode == "100") {
                    strSql.Append(string.Format(" and e.engineerstate='002' "));
                }
            }
            var data = this.BaseRepository().FindTable(strSql.ToString());
            OutprocessconfigService configService = new OutprocessconfigService();
            StartappprocessstatusService statusService = new StartappprocessstatusService();
            for (int i = data.Rows.Count - 1; i >= 0; i--)
            {
                var config = configService.GetEntityByModuleCode(data.Rows[i]["engineerid"].ToString(), mode); //根据项目查询每一个项目的流程配置项
                if (config == null)
                {
                    config = configService.GetEntityByModuleCode(currUser.OrganizeId, mode); //历史旧数据没有根据项目去配流程，所以根据电厂id去查询默认配置
                }
                if (config != null)
                {
                    if (!string.IsNullOrWhiteSpace(config.FrontModuleCode))
                    {
                        var listConfig = config.FrontModuleCode.Split(',');
                        var expression = LinqExtensions.True<StartappprocessstatusEntity>();
                        for (int j = 0; j < listConfig.Length; j++) //根据流程配置项查询外包流程流转表查询条件
                        {
                            switch (listConfig[j])
                            {
                                case "001"://单位资质
                                    expression = expression.And(t => t.EXAMSTATUS == "1");
                                    break;
                                case "002"://人员资质
                                    expression = expression.And(t => t.PEOPLESTATUS == "1");
                                    break;
                                case "003"://合同
                                    expression = expression.And(t => t.COMPACTSTATUS == "1");
                                    break;
                                case "004"://协议
                                    expression = expression.And(t => t.PACTSTATUS == "1");
                                    break;
                                case "005"://安全技术交底
                                    expression = expression.And(t => t.TECHNICALSTATUS == "1");
                                    break;
                                case "006"://三措两案
                                    expression = expression.And(t => t.THREETWOSTATUS == "1");
                                    break;
                                case "007"://工器具
                                    expression = expression.And(t => t.EQUIPMENTTOOLSTATUS == "1");
                                    break;
                                case "008"://入场许可
                                    expression = expression.And(t => t.SPTOOLSSTATUS == "1");
                                    break;
                                case "009"://开工申请
                                    break;
                                case "010"://保证金
                                    expression = expression.And(t => t.SECURITYSTATUS == "1");
                                    break;
                                default:
                                    break;
                            }
                        }
                        string OUTENGINEERID = data.Rows[i]["engineerid"].ToString();
                        expression = expression.And(t => t.OUTENGINEERID == OUTENGINEERID);
                        if (DbFactory.Base().FindEntity<StartappprocessstatusEntity>(expression) == null) //假如该项目流程流转不满足条件 则移除这条项目
                        {
                            data.Rows.RemoveAt(i);
                        }
                    }
                }
            }
            return data;
        }




        /// <summary>
        /// 根据当前登陆人和配置信息查询工程信息
        /// </summary>
        /// <param name="currUser">当前用户</param>
        /// <param name="mode">001--单位资质 002--人员资质 003--合同 004--协议 005--安全技术交底 006--三措两案 007--工器具验收  008--入场许可 009--开工申请 010--保证金 </param>
        /// <param name="orgid">电厂ID</param>
        /// <returns></returns>
        public DataTable GetEngineerDataByCondition(Operator currUser, string mode = "", string orgid = "") 
        {
            StringBuilder strSql = new StringBuilder();
            /*
             select e.engineername engineername,
                                         e.id engineerid,e.outprojectid unitid,d.fullname unitname,d.encode,
                                            engineerletdept deptname,engineerletdeptid deptid,engineercode projectcode,
                                            r.districtname areaname,f.itemname projecttype,g.itemname projectlevel,
                                            engineercontent projectcontent
                                    from epg_outsouringengineer e 
                                    left join epg_startappprocessstatus p on p.outengineerid=e.id
                                    left join base_department d on e.outprojectid=d.departmentid
                                    left join bis_district r on e.engineerarea=r.districtid
                                    left join base_dataitemdetail f on e.engineertype=f.itemvalue 
                                    left join base_dataitemdetail g on e.engineertype=g.itemvalue
                                    where f.itemid=(select itemid from base_dataitem where itemcode='ProjectType')
                                    and g.itemid=(select itemid from base_dataitem where itemcode='ProjectLevel')
             */
            strSql.AppendFormat(@"select  e.id engineerid,e.outprojectid unitid, e.usedeptpeople, e.engineerusedept,e.usedeptpeopphone,
	                                        e.engineerdirector,e.engineerdirectorphone,e.engineerletdeptid,
                                            e.engineercode,b.senddeptid,b.senddeptname,e.engineerletdept,e.engineerletpeople,e.engineerletpeoplephone,
                                           e.engineername,d.itemname engineertype,l.itemname engineerlevel,
                                           e.outprojectid,e.planenddate,e.actualenddate,e.predicttime,
                                           s.itemvalue statecode,s.itemname engineerstate,
                                           e.createdate,b.fullname outprojectname,e.engineerarea,
                                            decode(ss.examstatus, '1', '', '单位资质审查') examstatus,
                                           decode(ss.pactstatus, '1', '', '协议管理') pactstatus,
                                           decode(ss.compactstatus, '1', '', '合同管理') compactstatus,
                                           decode(ss.threetwostatus, '1', '', '三措两案') threetwostatus,
                                           decode(ss.technicalstatus, '1', '', '安全技术交底') technicalstatus,
                                           decode(ss.equipmenttoolstatus, '1', '', '工器具验收') equipmenttoolstatus,
                                            decode(ss.peoplestatus, '1', '', '人员资质审查') peoplestatus,
                                            i.busvalidstarttime,i.busvalidendtime,i.splvalidstarttime,e.createuserid,
                                            i.splvalidendtime,i.cqvalidstarttime,i.cqvalidendtime,e.isdeptadd
                                    from epg_outsouringengineer e
                                          left join base_department b on b.departmentid=e.outprojectid
                                          left join (select * from (select busvalidstarttime,outengineerid,busvalidendtime,
                                                                           splvalidstarttime,splvalidendtime,cqvalidstarttime,cqvalidendtime,
                                                                           row_number() over(partition by outengineerid order by createdate desc) rn
                                                          from epg_aptitudeinvestigateinfo)
                                                 where rn = 1) i on i.outengineerid = e.id
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectType') d on d.itemvalue=e.engineertype
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectLevel') l on l.itemvalue=e.engineerlevel
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='OutProjectState') s on s.itemvalue=e.engineerstate
                                          left join epg_startappprocessstatus ss on ss.outengineerid=e.id
                                    where ");
            if (!currUser.IsSystem)
            {
                if (currUser.IsSystem)
                {
                    strSql.Append("  1=1 ");
                }
                else if (currUser.RoleName.Contains("省级"))
                {
                    strSql.Append(string.Format(@" e.createuserorgcode  in (select encode
                        from base_department d
                        where d.deptcode like '{0}%' and d.nature = '厂级' and d.description is null)", currUser.NewDeptCode));
                }
                else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户"))
                {
                    strSql.Append(string.Format(" e.createuserorgcode like'%{0}%' ", currUser.OrganizeCode));
                }
                else if (currUser.RoleName.Contains("承包商级用户"))
                {
                    strSql.Append(string.Format("  e.outprojectid ='{0}'", currUser.DeptId));
                }
                else if (currUser.RoleName.Contains("专业级用户") || currUser.RoleName.Contains("班组级用户"))
                {
                    var pDept = new DepartmentService().GetParentDeptBySpecialArgs(currUser.ParentId, "部门");
                    strSql.Append(string.Format("  e.engineerletdeptid ='{0}'", pDept.DepartmentId));
                }
                else
                {
                    strSql.Append(string.Format("  e.engineerletdeptid ='{0}'", currUser.DeptId));
                }
            }
            if (!string.IsNullOrEmpty(mode))
            {
                var config = new OutprocessconfigService().GetEntityByModuleCode(currUser.OrganizeId, mode);

                if (config != null)
                {
                    if (!string.IsNullOrWhiteSpace(config.FrontModuleCode))
                    {
                        var listConfig = config.FrontModuleCode.Split(',');
                        for (int i = 0; i < listConfig.Length; i++)
                        {
                            switch (listConfig[i])
                            {
                                case "001"://单位资质
                                    strSql.Append(" and ss.examstatus='1'");
                                    break;
                                case "002"://人员资质
                                    strSql.Append(" and ss.peoplestatus='1'");
                                    break;
                                case "003"://合同
                                    strSql.Append(" and ss.compactstatus='1'");
                                    break;
                                case "004"://协议
                                    strSql.Append(" and ss.pactstatus='1'");
                                    break;
                                case "005"://安全技术交底
                                    strSql.Append(" and ss.technicalstatus='1'");
                                    break;
                                case "006"://三措两案
                                    strSql.Append(" and ss.threetwostatus='1'");
                                    break;
                                case "007"://工器具
                                    strSql.Append(" and ss.equipmenttoolstatus='1'");
                                    break;
                                case "008"://入场许可
                                    //strSql.Append(" and p.sptoolsstatus='1'");
                                    break;
                                case "009"://开工申请
                                    break;
                                case "010"://保证金
                                    strSql.Append(" and ss.securitystatus='1'");
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                if (mode == "009")
                {
                    strSql.Append(string.Format(" and e.id not in(select outengineerid from epg_startapplyfor）  "));
                }
                if (mode == "012")
                {
                    strSql.Append(string.Format(" and e.id not in(select projectid from epg_safetyevaluate）and e.engineerstate='002' "));
                }
                //在建状态
                if (mode == "100")
                {
                    strSql.Append(string.Format(" and e.engineerstate='002' "));
                }
            }

            return this.BaseRepository().FindTable(strSql.ToString());
        }

        /// <summary>
        /// 获取未停工或复工审核通过的在建工程信息
        /// </summary>
        /// <param name="currUser"></param>
        /// <returns></returns>
        public DataTable GetOnTheStock(Operator currUser)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select e.ENGINEERNAME ENGINEERNAME,
                                         e.id ENGINEERID
                                    from epg_outsouringengineer e 
                                    left join base_department b on b.departmentid=e.outprojectid
                                       where  e.isdeptadd=1 and e.engineerstate='002' and (e.stopreturnstate!='1' or e.stopreturnstate is null) ");
            if (currUser.IsSystem)
            {
                strSql.Append(" and 1=1 ");
            }
            else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户") || currUser.RoleName.Contains("公司管理员") || currUser.RoleName.Contains("公司领导"))
            {
                strSql.Append(string.Format(" and e.createuserorgcode like'%{0}%' ", currUser.OrganizeCode));
            }
            else if (currUser.RoleName.Contains("承包商级用户"))
            {
                strSql.Append(string.Format(" and e.outprojectid ='{0}'", currUser.DeptId));
            }
            else
            {
                strSql.Append(string.Format(" and e.engineerletdeptid='{0}'", currUser.DeptId));
            }
            //strSql.Append(string.Format(" and e.createuserorgcode like'%{0}%' ",currUser.OrganizeCode));
            return this.BaseRepository().FindTable(strSql.ToString());
        }

        /// <summary>
        /// 根据外包ID获取未停工的在建工程信息
        /// t.stopreturnstate!='1' 未停工的工程
        /// e.engineerstate='002' 开工申请已通过审核
        /// </summary>
        /// <param name="deptId">外包单位ID</param>
        /// <returns></returns>
        public DataTable GetEngineerDataByWBId(string deptId,string mode="")
        {
            StringBuilder strSql = new StringBuilder();
            var strWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(mode)) {
                strWhere = " and e.isdeptadd=1";
            }
            strSql.AppendFormat(@"select e.ENGINEERNAME ENGINEERNAME,
                                         e.id ENGINEERID
                                    from epg_outsouringengineer e 
                                       where  e.engineerstate='002' and (e.stopreturnstate!='1' or e.stopreturnstate is null) and e.outprojectid='{0}' {1}", deptId,strWhere);
            return this.BaseRepository().FindTable(strSql.ToString());
        }
        /// <summary>
        /// 获取工程的流程状态图
        /// </summary>
        /// <param name="keyValue">工程Id</param>
        /// <returns></returns>
        public Flow GetProjectFlow(string keyValue)
        {

            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            var isendflow = false;//流程结束标记
            Flow flow = new Flow();
            flow.title = "";
            flow.initNum = 22;
            Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var project = this.BaseRepository().FindEntity(keyValue);
            //工程状态为已开工或已完工则做完开工申请等于流程结束
            if (project.ENGINEERSTATE == "003")
            {
                isendflow = true;
            }
            int lastleft = 0;
            int lasttop = 0;
            string firstId = string.Empty;//第一个节点Id
            string lastId = string.Empty;//最后一个节点Id
            string startId = string.Empty;//流程开始节点
            string endId = string.Empty;//流程结束节点Id
            string firstCode = string.Empty;//第一个节点Code
            string lastCode = "011";//设置结束节点(流程结束的前一个节点)
            if (project != null)
            {
                //查询电厂的外包流程配置
                string sql = string.Format(@"select t.id,t.modulename,t.modulecode,t.frontmodulecode,t.frontmodulename,t.deptcode,t.address from wf_schemecontent a left join epg_outprocessconfig t on a.id=t.recid where a.wfschemeinfoid='{0}'  order by t.modulecode ", keyValue);
                DataTable dt = this.BaseRepository().FindTable(sql);
                if (dt.Rows.Count == 0) //如果根据工程没有查到外包流程，可能是旧数据，则使用旧逻辑 根据单位查询流程
                {
                    sql = string.Format(@"select t.id,t.modulename,t.modulecode,t.frontmodulecode,t.frontmodulename,t.deptcode,t.address from wf_schemecontent a left join epg_outprocessconfig t on a.id=t.recid where a.wfschemeinfoid='{0}'  order by t.modulecode ", new DepartmentService().GetEntityByCode(project.CREATEUSERORGCODE).DepartmentId);
                    dt = this.BaseRepository().FindTable(sql);
                }
                //确认电厂第一步流程和开工申请流程
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulecode"].ToString()))
                    {
                        firstCode = dt.Rows[i]["modulecode"].ToString();
                    }
                }
                //如果有配置则查询流程状态(这里不需要判断,前端判断了没有流程配置是不能进外包模块的)
                string sqlStatus = string.Format(@"select t.examstatus,t.securitystatus,t.pactstatus,t.technicalstatus,t.threetwostatus,
                                            t.equipmenttoolstatus,t.peoplestatus,t.compactstatus,t.sptoolsstatus from EPG_STARTAPPPROCESSSTATUS t where t.outengineerid='{0}'", project.ID);
                DataTable dtStatus = this.BaseRepository().FindTable(sqlStatus);

                //流程开始节点
                nodes nodes_start = new nodes();
                nodes_start.alt = true;
                nodes_start.isclick = false;
                nodes_start.css = "";
                nodes_start.id = Guid.NewGuid().ToString();
                nodes_start.img = "";
                nodes_start.name = "流程开始";
                nodes_start.type = "startround";
                nodes_start.width = 150;
                nodes_start.height = 60;
                nodes_start.left = 150;
                nodes_start.top = 150;
                startId = nodes_start.id;
                nlist.Add(nodes_start);
                setInfo sinfostart = new setInfo();
                sinfostart.NodeName = nodes_start.name;
                sinfostart.Taged = 1;
                nodes_start.setInfo = sinfostart;

                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        nodes nodes = new nodes();
                        nodes.alt = true;
                        nodes.isclick = false;
                        nodes.css = "";
                        nodes.id = dr["id"].ToString(); //主键
                        nodes.img = "";
                        nodes.name = dr["modulename"].ToString();

                        nodes.type = "stepnode";
                        nodes.width = 150;
                        nodes.height = 60;
                        nodes.url = dr["address"].IsEmpty() ? "" : dr["address"].ToString();

                        int m = i % 5;
                        int n = i / 5;

                        if (dr["modulecode"].ToString() == firstCode)
                        {
                            nodes.left = 150;
                            nodes.top = 300;
                            firstId = nodes.id;
                        }
                        else if (dr["modulecode"].ToString() == lastCode)
                        {
                            nodes.left = 150 + (250 * (n + 1)) + 300;
                            nodes.top = 300;
                            lastleft = nodes.left;
                            lasttop = nodes.top;
                            lastId = nodes.id;
                        }
                        else
                        {
                            nodes.left = 150 + (250 * (n + 1));
                            if (Convert.ToInt32(dr["modulecode"].ToString()) > Convert.ToInt32(lastCode))
                            {
                                nodes.top = 70 + (m - 2) * 80;
                            }
                            else
                            {
                                nodes.top = 70 + (m - 1) * 80;
                            }

                        }
                        setInfo sinfo = new setInfo();
                        sinfo.NodeName = nodes.name;
                        if (dtStatus.Rows.Count > 0)
                        {
                            #region 资质审查状态(0:未完成1:完成)
                            if (dt.Rows[i]["modulecode"].ToString() == "001")
                            {
                                if (dtStatus.Rows[0]["examstatus"].ToString() == "1")
                                {
                                    var info = new AptitudeinvestigateinfoService().GetList("").Where(x => x.OUTENGINEERID == project.ID && x.ISSAVEORCOMMIT == "1" && x.ISAUDITOVER == "1").ToList();
                                    sinfo.Taged = 1;
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    for (int k = 0; k < info.Count; k++)
                                    {
                                        var auditList = new AptitudeinvestigateauditService().GetAuditList(info[k].ID);
                                        foreach (AptitudeinvestigateauditEntity item in auditList)
                                        {
                                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                            DateTime auditdate;
                                            DateTime.TryParse(item.AUDITTIME.ToString(), out auditdate);
                                            nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                            nodedesignatedata.creatdept = item.AUDITDEPT.ToString();
                                            nodedesignatedata.createuser = item.AUDITPEOPLE.ToString();
                                            nodedesignatedata.status = item.AUDITRESULT.ToString() == "0" ? "通过" : "不通过";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }
                                    }
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                                else
                                {
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    var info = new AptitudeinvestigateinfoService().GetList("").Where(x => x.OUTENGINEERID == project.ID && x.ISSAVEORCOMMIT == "1");
                                    if (info.Count() > 0)
                                    {
                                        sinfo.Taged = 0;
                                        foreach (AptitudeinvestigateinfoEntity itInfo in info)
                                        {
                                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                            DateTime auditdate;
                                            DateTime.TryParse(itInfo.CREATEDATE.ToString(), out auditdate);
                                            nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                            nodedesignatedata.creatdept = new DepartmentService().GetList().Where(x => x.EnCode == itInfo.CREATEUSERDEPTCODE).FirstOrDefault().FullName;
                                            nodedesignatedata.createuser = itInfo.CREATEUSERNAME;
                                            nodedesignatedata.status = "审核中";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }
                                    }
                                    else
                                    {
                                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                        nodedesignatedata.createdate = "无";
                                        nodedesignatedata.creatdept = "";
                                        nodedesignatedata.createuser = "";
                                        nodedesignatedata.status = "无";
                                        nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                        nodelist.Add(nodedesignatedata);
                                    }
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }
                            #endregion
                            #region 安全保证金状态(0:未完成1:完成)
                            if (dt.Rows[i]["modulecode"].ToString() == "010")
                            {
                                if (dtStatus.Rows[0]["securitystatus"].ToString() == "1")
                                {
                                    var safeMoney = new SafetyEamestMoneyService().GetList().Where(x => x.PROJECTID == project.ID && x.ISSEND == "1").ToList();
                                    sinfo.Taged = 1;
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    for (int k = 0; k < safeMoney.Count; k++)
                                    {
                                        var auditList = new AptitudeinvestigateauditService().GetAuditList(safeMoney[k].ID);
                                        foreach (AptitudeinvestigateauditEntity item in auditList)
                                        {
                                            if (item.AUDITRESULT.ToString() == "2") {
                                                continue;
                                            }
                                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                            if (item.AUDITTIME==null)
                                            {
                                                nodedesignatedata.createdate = "";
                                            }
                                            else {
                                                DateTime auditdate;
                                                DateTime.TryParse(item.AUDITTIME.ToString(), out auditdate);
                                                nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                            }
                                            nodedesignatedata.creatdept = string.IsNullOrWhiteSpace(item.AUDITDEPT) ? "" : item.AUDITDEPT.ToString();
                                            nodedesignatedata.createuser = string.IsNullOrWhiteSpace(item.AUDITPEOPLE) ? "" : item.AUDITPEOPLE.ToString();
                                            nodedesignatedata.status = item.AUDITRESULT.ToString() == "0" ? "通过" : "不通过";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }
                                    }
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                                else
                                {
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                    nodedesignatedata.createdate = "无";
                                    nodedesignatedata.creatdept = "";
                                    nodedesignatedata.createuser = "";
                                    nodedesignatedata.status = "无";
                                    nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                    nodelist.Add(nodedesignatedata);
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }
                            #endregion
                            #region 协议状态(0:未完成1:完成)
                            if (dt.Rows[i]["modulecode"].ToString() == "004")
                            {
                                if (dtStatus.Rows[0]["pactstatus"].ToString() == "1")
                                {
                                    var protocolInfo = new ProtocolService().GetList().Where(x => x.PROJECTID == project.ID).ToList();
                                    sinfo.Taged = 1;
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();

                                    foreach (ProtocolEntity item in protocolInfo)
                                    {
                                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                        DateTime auditdate;
                                        DateTime.TryParse(item.CREATEDATE.Value.ToString(), out auditdate);
                                        nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                        nodedesignatedata.creatdept = new DepartmentService().GetList().Where(x => x.EnCode == item.CREATEUSERDEPTCODE.ToString()).FirstOrDefault().FullName;
                                        nodedesignatedata.createuser = item.CREATEUSERNAME;
                                        nodedesignatedata.status = "通过";
                                        nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                        nodelist.Add(nodedesignatedata);
                                    }

                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                                else
                                {
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                    nodedesignatedata.createdate = "无";
                                    nodedesignatedata.creatdept = "无";
                                    nodedesignatedata.createuser = "无";
                                    nodedesignatedata.status = "无";
                                    nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                    nodelist.Add(nodedesignatedata);
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }
                            #endregion
                            #region 安全技术交底状态(0:未完成1:完成)
                            if (dt.Rows[i]["modulecode"].ToString() == "005")
                            {
                                if (dtStatus.Rows[0]["technicalstatus"].ToString() == "1")
                                {
                                    var techdisclosure = new TechDisclosureService().GetList().Where(x => x.PROJECTID == project.ID).ToList();
                                    sinfo.Taged = 1;
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    if (techdisclosure.Count > 0)
                                    {
                                        foreach (TechDisclosureEntity item in techdisclosure)
                                        {
                                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                            DateTime auditdate;
                                            DateTime.TryParse(item.CREATEDATE.ToString(), out auditdate);
                                            nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                            nodedesignatedata.creatdept = new DepartmentService().GetList().Where(x => x.EnCode == item.CREATEUSERDEPTCODE.ToString()).FirstOrDefault().FullName;
                                            nodedesignatedata.createuser = item.CREATEUSERNAME.ToString();
                                            nodedesignatedata.status = "通过";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }
                                    }
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                                else
                                {
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                    nodedesignatedata.createdate = "无";
                                    nodedesignatedata.creatdept = "";
                                    nodedesignatedata.createuser = "";
                                    nodedesignatedata.status = "无";
                                    nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                    nodelist.Add(nodedesignatedata);
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }
                            #endregion
                            #region 三措两案状态(0:未完成1:完成)
                            if (dt.Rows[i]["modulecode"].ToString() == "006")
                            {
                                if (dtStatus.Rows[0]["threetwostatus"].ToString() == "1")
                                {
                                    var schememeasure = new SchemeMeasureService().GetList().Where(x => x.PROJECTID == project.ID && x.ISSAVED == "1" && x.ISOVER == "1").ToList();
                                    sinfo.Taged = 1;
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    if (schememeasure.Count > 0)
                                    {

                                        for (int k = 0; k < schememeasure.Count; k++)
                                        {
                                            var auditList = new AptitudeinvestigateauditService().GetAuditList(schememeasure[k].ID);
                                            foreach (AptitudeinvestigateauditEntity item in auditList)
                                            {
                                                NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                                DateTime auditdate;
                                                DateTime.TryParse(item.AUDITTIME.ToString(), out auditdate);
                                                nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                                nodedesignatedata.creatdept = item.AUDITDEPT.ToString();
                                                nodedesignatedata.createuser = item.AUDITPEOPLE.ToString();
                                                nodedesignatedata.status = item.AUDITRESULT.ToString() == "0" ? "通过" : "不通过";
                                                nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                                nodelist.Add(nodedesignatedata);
                                            }
                                        }
                                    }
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                                else
                                {
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();

                                    var schememeasure = new SchemeMeasureService().GetList().Where(x => x.PROJECTID == project.ID && x.ISSAVED == "1").ToList();
                                    if (schememeasure.Count() > 0)
                                    {
                                        sinfo.Taged = 0;
                                        foreach (SchemeMeasureEntity itInfo in schememeasure)
                                        {
                                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                            nodedesignatedata.createdate = itInfo.CREATEDATE.Value.ToString("yyyy-MM-dd HH:mm");
                                            nodedesignatedata.creatdept = new DepartmentService().GetList().Where(x => x.EnCode == itInfo.CREATEUSERDEPTCODE).FirstOrDefault().FullName;
                                            nodedesignatedata.createuser = itInfo.CREATEUSERNAME;
                                            nodedesignatedata.status = "审核中";
                                            //nodedesignatedata.nextuser = itInfo.FLOWDEPTNAME + itInfo.FLOWROLENAME;
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }
                                    }
                                    else
                                    {
                                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                        nodedesignatedata.createdate = "无";
                                        nodedesignatedata.creatdept = "";
                                        nodedesignatedata.createuser = "";
                                        nodedesignatedata.status = "无";
                                        nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                        nodelist.Add(nodedesignatedata);
                                    }
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }
                            #endregion
                            #region 设备工器具验收状态(0:未完成1:完成)
                            if (dt.Rows[i]["modulecode"].ToString() == "007")
                            {
                                if (dtStatus.Rows[0]["equipmenttoolstatus"].ToString() == "1")
                                {
                                    var toolsInfo = new ToolsService().GetList("").Where(x => x.OUTENGINEERID == project.ID && x.ISSAVED == "1" && x.ISOVER == "1").ToList();
                                    sinfo.Taged = 1;
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    for (int k = 0; k < toolsInfo.Count; k++)
                                    {
                                        var auditList = new ToolsAuditService().GetList("").Where(x => x.TOOLSID == toolsInfo[k].TOOLSID);
                                        foreach (ToolsAuditEntity item in auditList)
                                        {
                                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                            DateTime auditdate;
                                            DateTime.TryParse(item.AUDITTIME.ToString(), out auditdate);
                                            nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                            nodedesignatedata.creatdept = item.AUDITDEPT.ToString();
                                            nodedesignatedata.createuser = item.AUDITPEOPLE.ToString();
                                            nodedesignatedata.status = item.AUDITRESULT.ToString() == "0" ? "通过" : "不通过";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }
                                    }
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                                else
                                {
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    var toolsInfo = new ToolsService().GetList("").Where(x => x.OUTENGINEERID == project.ID && x.ISSAVED == "1").ToList();
                                    if (toolsInfo.Count > 0)
                                    {
                                        sinfo.Taged = 0;
                                        foreach (ToolsEntity itInfo in toolsInfo)
                                        {
                                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                            nodedesignatedata.createdate = itInfo.CREATEDATE.Value.ToString("yyyy-MM-dd HH:mm");
                                            nodedesignatedata.creatdept = new DepartmentService().GetList().Where(x => x.EnCode == itInfo.CREATEUSERDEPTCODE).FirstOrDefault().FullName;
                                            nodedesignatedata.createuser = itInfo.CREATEUSERNAME;
                                            nodedesignatedata.status = "审核中";
                                            //nodedesignatedata.nextuser = itInfo.FLOWDEPTNAME + itInfo.FLOWROLENAME;
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }

                                    }
                                    else
                                    {
                                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                        nodedesignatedata.createdate = "无";
                                        nodedesignatedata.creatdept = "";
                                        nodedesignatedata.createuser = "";
                                        nodedesignatedata.status = "无";
                                        nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                        nodelist.Add(nodedesignatedata);
                                    }
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }

                            #endregion
                            #region 人员审查状态(0:未完成1:完成)
                            if (dt.Rows[i]["modulecode"].ToString() == "002")
                            {
                                if (dtStatus.Rows[0]["peoplestatus"].ToString() == "1")
                                {
                                    var peopleInfo = new PeopleReviewService().GetList("").Where(x => x.OUTENGINEERID == project.ID && x.ISSAVEORCOMMIT == "1" && x.ISAUDITOVER == "1").ToList();
                                    sinfo.Taged = 1;
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    for (int k = 0; k < peopleInfo.Count; k++)
                                    {
                                        var auditList = new AptitudeinvestigateauditService().GetAuditList(peopleInfo[k].ID);
                                        foreach (AptitudeinvestigateauditEntity item in auditList)
                                        {
                                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                            DateTime auditdate;
                                            DateTime.TryParse(item.AUDITTIME.ToString(), out auditdate);
                                            nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                            nodedesignatedata.creatdept = item.AUDITDEPT.ToString();
                                            nodedesignatedata.createuser = item.AUDITPEOPLE.ToString();
                                            nodedesignatedata.status = item.AUDITRESULT.ToString() == "0" ? "通过" : "不通过";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }
                                    }
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                                else
                                {
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    var peopleInfo = new PeopleReviewService().GetList("").Where(x => x.OUTENGINEERID == project.ID && x.ISSAVEORCOMMIT == "1");
                                    if (peopleInfo.Count() > 0)
                                    {
                                        sinfo.Taged = 0;
                                        foreach (PeopleReviewEntity itInfo in peopleInfo)
                                        {
                                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                            nodedesignatedata.createdate = "无";
                                            nodedesignatedata.creatdept = new DepartmentService().GetList().Where(x => x.EnCode == itInfo.CREATEUSERDEPTCODE).FirstOrDefault().FullName;
                                            nodedesignatedata.createuser = itInfo.CREATEUSERNAME;
                                            nodedesignatedata.status = "无";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }
                                    }
                                    else
                                    {
                                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                        nodedesignatedata.createdate = "无";
                                        nodedesignatedata.creatdept = "";
                                        nodedesignatedata.createuser = "";
                                        nodedesignatedata.status = "无";
                                        nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                        nodelist.Add(nodedesignatedata);
                                    }
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }
                            #endregion
                            #region 合同状态(0:未完成1:完成)
                            if (dt.Rows[i]["modulecode"].ToString() == "003")
                            {
                                if (dtStatus.Rows[0]["compactstatus"].ToString() == "1")
                                {
                                    var comInfo = new CompactService().GetList().Where(x => x.PROJECTID == project.ID).ToList();
                                    sinfo.Taged = 1;
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();

                                    foreach (CompactEntity item in comInfo)
                                    {
                                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                        DateTime auditdate;
                                        DateTime.TryParse(item.CREATEDATE.Value.ToString(), out auditdate);
                                        nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                        nodedesignatedata.creatdept = new DepartmentService().GetList().Where(x => x.EnCode == item.CREATEUSERDEPTCODE.ToString()).FirstOrDefault().FullName;
                                        nodedesignatedata.createuser = item.CREATEUSERNAME;
                                        nodedesignatedata.status = "通过";
                                        nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                        nodelist.Add(nodedesignatedata);
                                    }

                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                                else
                                {
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                    nodedesignatedata.createdate = "无";
                                    nodedesignatedata.creatdept = "无";
                                    nodedesignatedata.createuser = "无";
                                    nodedesignatedata.status = "无";
                                    nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                    nodelist.Add(nodedesignatedata);
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }
                            #endregion
                            #region 入厂许可
                            if (dt.Rows[i]["modulecode"].ToString() == "008")
                            {
                                List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                var IntromInfo = new IntromissionService().GetList("").Where(x => x.OUTENGINEERID == project.ID && x.INVESTIGATESTATE == "3").ToList();
                                if (IntromInfo.Count > 0)
                                {
                                    sinfo.Taged = 1;
                                    for (int k = 0; k < IntromInfo.Count; k++)
                                    {
                                        var auditList = new AptitudeinvestigateauditService().GetList("").Where(x => x.APTITUDEID == IntromInfo[k].ID);
                                        foreach (AptitudeinvestigateauditEntity item in auditList)
                                        {
                                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                            DateTime auditdate;
                                            DateTime.TryParse(item.AUDITTIME.Value.ToString(), out auditdate);
                                            nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                            nodedesignatedata.creatdept = item.AUDITDEPT;
                                            nodedesignatedata.createuser = item.AUDITPEOPLE;
                                            nodedesignatedata.status = "通过";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }
                                    }
                                }
                                else
                                {
                                    var IntromInfo1 = new IntromissionService().GetList("").Where(x => x.OUTENGINEERID == project.ID&&x.INVESTIGATESTATE!="0").ToList();
                                    if (IntromInfo1.Count > 0)
                                    {
                                        sinfo.Taged = 0;
                                        foreach (IntromissionEntity item in IntromInfo1)
                                        {
                                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                            DateTime auditdate;
                                            DateTime.TryParse(item.CREATEDATE.Value.ToString(), out auditdate);
                                            nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                            nodedesignatedata.creatdept = new DepartmentService().GetList().Where(x => x.EnCode == item.CREATEUSERDEPTCODE).FirstOrDefault().FullName;
                                            nodedesignatedata.createuser = item.CREATEUSERNAME;
                                            nodedesignatedata.status = item.INVESTIGATESTATE == "1" ? "审查中" : "审核中";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }
                                    }
                                    else
                                    {
                                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                        nodedesignatedata.createdate = "无";
                                        nodedesignatedata.creatdept = "";
                                        nodedesignatedata.createuser = "";
                                        nodedesignatedata.status = "无";
                                        nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                        nodelist.Add(nodedesignatedata);
                                    }
                                }
                                sinfo.NodeDesignateData = nodelist;
                                nodes.setInfo = sinfo;
                            }
                            #endregion
                            #region 开工申请
                            if (dt.Rows[i]["modulecode"].ToString() == "009")
                            {
                                List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                var startapply = new StartapplyforService().GetList("").Where(x => x.OUTENGINEERID == project.ID && x.ISCOMMIT == "1" && x.IsOver == 1).ToList();
                                if (startapply.Count > 0)
                                {
                                    sinfo.Taged = 1;
                                    for (int k = 0; k < startapply.Count; k++)
                                    {
                                        var auditList = new AptitudeinvestigateauditService().GetList("").Where(x => x.APTITUDEID == startapply[k].ID);
                                        foreach (AptitudeinvestigateauditEntity item in auditList)
                                        {
                                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                            DateTime auditdate;
                                            DateTime.TryParse(item.AUDITTIME.Value.ToString(), out auditdate);
                                            nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                            nodedesignatedata.creatdept = item.AUDITDEPT;
                                            nodedesignatedata.createuser = item.AUDITPEOPLE;
                                            nodedesignatedata.status = "通过";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }
                                    }
                                }
                                else
                                {
                                    var startapply1 = new StartapplyforService().GetList("").Where(x => x.OUTENGINEERID == project.ID && x.ISCOMMIT == "1").ToList();
                                    if (startapply1.Count > 0)
                                    {
                                        sinfo.Taged = 0;
                                        foreach (StartapplyforEntity item in startapply1)
                                        {
                                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                            DateTime auditdate;
                                            DateTime.TryParse(item.CREATEDATE.Value.ToString(), out auditdate);
                                            nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                            nodedesignatedata.creatdept = new DepartmentService().GetList().Where(x => x.EnCode == item.CREATEUSERDEPTCODE).FirstOrDefault().FullName;
                                            nodedesignatedata.createuser = item.CREATEUSERNAME;
                                            nodedesignatedata.status = item.ISINVESTOVER == 1 ? "审核中" : "审查中";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }
                                    }
                                    else
                                    {
                                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                        nodedesignatedata.createdate = "无";
                                        nodedesignatedata.creatdept = "";
                                        nodedesignatedata.createuser = "";
                                        nodedesignatedata.status = "无";
                                        nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                        nodelist.Add(nodedesignatedata);
                                    }
                                }
                                sinfo.NodeDesignateData = nodelist;
                                nodes.setInfo = sinfo;
                            }
                            #endregion
                            #region 安全评价
                            if (dt.Rows[i]["modulecode"].ToString() == "011")
                            {
                                var safetyevaluate = new SafetyEvaluateService().GetList().Where(x => x.PROJECTID == project.ID && x.ISSEND == "1").ToList();
                                if (safetyevaluate.Count() > 0)
                                {
                                    sinfo.Taged = 1;
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                    if (safetyevaluate[0].CREATEDATE == null)
                                    {
                                        nodedesignatedata.createdate = "";
                                    }
                                    else
                                    {
                                        nodedesignatedata.createdate =Convert.ToDateTime(safetyevaluate[0].CREATEDATE).ToString("yyyy-MM-dd HH:mm");
                                    }
                                    nodedesignatedata.creatdept = string.IsNullOrWhiteSpace(safetyevaluate[0].CREATEUSERDEPTCODE) ? "" : new DepartmentService().GetEntityByCode(safetyevaluate[0].CREATEUSERDEPTCODE).FullName;
                                    nodedesignatedata.createuser = string.IsNullOrWhiteSpace(safetyevaluate[0].CREATEUSERNAME) ? "" : safetyevaluate[0].CREATEUSERNAME;
                                    nodedesignatedata.status = "通过" ;
                                    nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                    nodelist.Add(nodedesignatedata);
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                                else
                                {
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                    nodedesignatedata.createdate = "无";
                                    nodedesignatedata.creatdept = "";
                                    nodedesignatedata.createuser = "";
                                    nodedesignatedata.status = "无";
                                    nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "无" : dt.Rows[i]["frontmodulename"].ToString();
                                    nodelist.Add(nodedesignatedata);
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }
                            #endregion
                        }
                        nlist.Add(nodes);
                    }
                    //流程结束节点
                    nodes nodes_end = new nodes();
                    nodes_end.alt = true;
                    nodes_end.isclick = false;
                    nodes_end.css = "";
                    nodes_end.id = Guid.NewGuid().ToString();
                    nodes_end.img = "";
                    nodes_end.name = "流程结束";
                    nodes_end.type = "endround";
                    nodes_end.width = 150;
                    nodes_end.height = 60;
                    //取最后一流程的位置，相对排位
                    nodes_end.left = lastleft;
                    nodes_end.top = lasttop + 100;
                    endId = nodes_end.id;
                    nlist.Add(nodes_end);

                    //如果状态为审核通过或不通过，流程结束进行标识 
                    if (isendflow)
                    {
                        setInfo sinfo = new setInfo();
                        sinfo.NodeName = nodes_end.name;
                        sinfo.Taged = 1;
                        //List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        //NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        //nodelist.Add(nodedesignatedata);
                        //sinfo.NodeDesignateData = nodelist;
                        nodes_end.setInfo = sinfo;
                    }
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        for (int m = 0; m < dt.Rows.Count; m++)
                        {
                            if (string.IsNullOrWhiteSpace(dt.Rows[j]["frontmodulecode"].ToString()))
                            {
                                lines lines = new lines();
                                lines.alt = true;
                                lines.id = Guid.NewGuid().ToString();
                                lines.from = startId;
                                lines.to = dt.Rows[j]["id"].ToString();
                                lines.name = "";
                                lines.type = "sl";
                                llist.Add(lines);
                            }
                            else
                            {
                                if (dt.Rows[j]["frontmodulecode"].ToString().Contains(dt.Rows[m]["modulecode"].ToString()))
                                {
                                    lines lines = new lines();
                                    lines.alt = true;
                                    lines.id = Guid.NewGuid().ToString();
                                    lines.from = dt.Rows[m]["id"].ToString();
                                    lines.to = dt.Rows[j]["id"].ToString();
                                    lines.name = "";
                                    lines.type = "sl";
                                    llist.Add(lines);
                                }
                            }
                        }
                    }
                    lines lines_end = new lines();
                    lines_end.alt = true;
                    lines_end.id = Guid.NewGuid().ToString();
                    lines_end.from = lastId;
                    lines_end.to = endId;
                    llist.Add(lines_end);
                    flow.nodes = nlist;
                    flow.lines = llist;
                }
            }
            return flow;
        }
        /// <summary>
        /// 根据当前登录人 
        /// 获取已经停工的工程信息
        /// 获取还没有添加复工的工程
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        public DataTable GetStopEngineerList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select e.engineername engineername,e.id engineerid from epg_outsouringengineer e
                                left join base_department b on b.departmentid=e.outprojectid");
            Operator currUser = OperatorProvider.Provider.Current();
            if (currUser.IsSystem)
            {
                strSql.Append(" where 1=1 ");
            }
            else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户") || currUser.RoleName.Contains("公司管理员") || currUser.RoleName.Contains("公司领导"))
            {
                strSql.Append(string.Format(" where e.createuserorgcode like'%{0}%' ", currUser.OrganizeCode));
            }
            else if (currUser.RoleName.Contains("承包商级用户"))
            {
                strSql.Append(string.Format(" where e.outprojectid ='{0}'", currUser.DeptId));
            }
            else
            {
                strSql.Append(string.Format(" where e.engineerletdeptid='{0}' ", currUser.DeptId));
            }
            strSql.Append(@"  and e.stopreturnstate='1' and e.id not in (select outengineerid from epg_returntowork  r
left join epg_aptitudeinvestigateaudit a on r.id=a.aptitudeid where a.auditresult='1' or a.auditresult is null) ");//停工的工程或已经添加复工的工程
            return this.BaseRepository().FindTable(strSql.ToString());
        }
        /// <summary>
        /// 外包工程状态统计图
        /// </summary>
        /// <param name="deptid">部门Id</param>
        /// <param name="year">时间</param>
        /// <param name="state">状态值001 未开工 002 在建 003 已完工</param>
        /// <returns></returns>
        public string GetStateCount(string deptid, string year = "", string state = "001,002,003")
        {
            List<object[]> list = new List<object[]>();
            Dictionary<string, int> dic = new Dictionary<string, int>();
            string sql = string.Format(@" select count(*) num, d.itemname,d.itemvalue
                                                  from epg_outsouringengineer t
                                                  left join (select m.itemname, m.itemvalue
                                                               from base_dataitem t
                                                               left join base_dataitemdetail m on m.itemid = t.itemid
                                                              where t.itemcode = 'OutProjectState') d on d.itemvalue =t.engineerstate
                                                  left join Base_Department b on b.departmentid = t.outprojectid where t.isdeptadd=1 ");
            if (string.IsNullOrEmpty(deptid))
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (user.RoleName.Contains("厂级部门用户") || user.RoleName.Contains("公司级用户"))
                {
                    sql += string.Format(" and 1=1 ");
                }
                else
                {
                    sql += string.Format(" and t.engineerletdeptid='{0}' ", user.DeptId);
                }
                sql += string.Format(" and t.createuserorgcode like'%{0}%'", user.OrganizeCode);
            }
            else
                sql += string.Format(" and b.departmentid='{0}'", deptid);
            if (!string.IsNullOrEmpty(year))
            {
                string startTime = new DateTime(Convert.ToInt32(year), 1, 1).ToString();
                string endTime = new DateTime(Convert.ToInt32(year), 12, 31).ToString();
                sql += string.Format(" and (t.createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and t.createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(startTime).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(endTime).ToString("yyyy-MM-dd 23:59:59"));
            }
            sql += " group by d.itemvalue,d.itemname";
            DataTable dt = this.BaseRepository().FindTable(sql);

            int count = dt.Select("itemvalue='001'").Length == 0 ? 0 : int.Parse(dt.Select("itemvalue='001'")[0][0].ToString());
            object[] arr = { "未开工", count };
            list.Add(arr);
            count = dt.Select("itemvalue='002'").Length == 0 ? 0 : int.Parse(dt.Select("itemvalue='002'")[0][0].ToString());
            arr = new object[] { "在建", count };
            list.Add(arr);
            count = dt.Select("itemvalue='003'").Length == 0 ? 0 : int.Parse(dt.Select("itemvalue='003'")[0][0].ToString());
            arr = new object[] { "已完工", count };
            list.Add(arr);
            dt.Dispose();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// 外包工程状态统计表
        /// </summary>
        /// <param name="deptid">部门Id</param>
        /// <param name="year">时间</param>
        /// <param name="state">状态值001 未开工 002 在建 003 已完工</param>
        /// <returns></returns>
        public string GetStateList(string deptid, string year = "", string state = "001,002,003")
        {
            List<OutEngineerStatEntity> list = new List<OutEngineerStatEntity>();
            Dictionary<string, int> dic = new Dictionary<string, int>();
            string sql = string.Format(@" select count(*) num, d.itemname,d.itemvalue
                                                  from epg_outsouringengineer t
                                                  left join (select m.itemname, m.itemvalue
                                                               from base_dataitem t
                                                               left join base_dataitemdetail m on m.itemid = t.itemid
                                                              where t.itemcode = 'OutProjectState') d on d.itemvalue =t.engineerstate
                                                  left join Base_Department b on b.departmentid = t.outprojectid where t.isdeptadd=1 ");
            if (string.IsNullOrEmpty(deptid))
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (user.RoleName.Contains("厂级部门用户") || user.RoleName.Contains("公司级用户"))
                {
                    sql += string.Format(" and 1=1 ");
                }
                else
                {
                    sql += string.Format(" and t.engineerletdeptid='{0}' ", user.DeptId);
                }
                sql += string.Format(" and t.createuserorgcode like'%{0}%'", user.OrganizeCode);
            }
            else
                sql += string.Format(" and b.departmentid='{0}'", deptid);
            if (!string.IsNullOrEmpty(year))
            {
                string startTime = new DateTime(Convert.ToInt32(year), 1, 1).ToString();
                string endTime = new DateTime(Convert.ToInt32(year), 12, 31).ToString();
                sql += string.Format(" and (t.createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and t.createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(startTime).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(endTime).ToString("yyyy-MM-dd 23:59:59"));
            }
            sql += " group by d.itemvalue,d.itemname";
            DataTable dt = this.BaseRepository().FindTable(sql);

            int count1 = dt.Select("itemvalue='001'").Length == 0 ? 0 : int.Parse(dt.Select("itemvalue='001'")[0][0].ToString());
            int count2 = dt.Select("itemvalue='002'").Length == 0 ? 0 : int.Parse(dt.Select("itemvalue='002'")[0][0].ToString());
            int count3 = dt.Select("itemvalue='003'").Length == 0 ? 0 : int.Parse(dt.Select("itemvalue='003'")[0][0].ToString());
            int sum = count1 + count2 + count3;

            decimal percent1 = (sum == 0 ? 0 : decimal.Parse(count1.ToString()) / sum);
            percent1 = percent1 == 0 ? 0 : Math.Round(percent1 * 100, 2);

            decimal percent2 = (sum == 0 ? 0 : decimal.Parse(count2.ToString()) / sum);
            percent2 = percent2 == 0 ? 0 : Math.Round(percent2 * 100, 2);

            decimal percent3 = (sum == 0 ? 0 : decimal.Parse(count3.ToString()) / sum);
            percent3 = percent3 == 0 ? 0 : Math.Round(percent3 * 100, 2);

            decimal percent4 = (sum == 0 ? 0 : decimal.Parse(sum.ToString()) / sum);
            percent4 = percent4 == 0 ? 0 : Math.Round(percent4 * 100, 2);

            OutEngineerStatEntity e1 = new OutEngineerStatEntity() { name = "未开工", value = "001", num = count1, percent = percent1 };
            list.Add(e1);

            OutEngineerStatEntity e2 = new OutEngineerStatEntity() { name = "在建", value = "002", num = count2, percent = percent2 };
            list.Add(e2);

            OutEngineerStatEntity e3 = new OutEngineerStatEntity() { name = "已完工", value = "003", num = count3, percent = percent3 };
            list.Add(e3);

            OutEngineerStatEntity e4 = new OutEngineerStatEntity() { name = "工程总数", value = "", num = sum, percent = percent4 };
            list.Add(e4);

            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = list.Count,
                rows = list
            });
        }

        /// <summary>
        /// 根据部门获取工程类型数量统计图表数据
        /// </summary>
        /// <param name="deptid">部门id</param>
        /// <param name="year">统计年份</param>
        /// <param name="type">工程类型001 长协 002 外包</param>
        /// <returns></returns>
        public string GetTypeCount(string deptid, string year = "", string type = "001,002")
        {
            List<object[]> list = new List<object[]>();
            Dictionary<string, int> dic = new Dictionary<string, int>();
            string sql = string.Format(@" select count(*) num, d.itemname,d.itemvalue
                                                  from epg_outsouringengineer t
                                                  left join (select m.itemname, m.itemvalue
                                                               from base_dataitem t
                                                               left join base_dataitemdetail m on m.itemid = t.itemid
                                                              where t.itemcode = 'ProjectType') d on d.itemvalue =t.engineertype
                                                  left join Base_Department b on b.departmentid = t.outprojectid where t.isdeptadd=1 ");
            if (string.IsNullOrEmpty(deptid))
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (user.RoleName.Contains("厂级部门用户") || user.RoleName.Contains("公司级用户"))
                {
                    //sql += string.Format("  1=1 ");
                }
                else
                {
                    sql += string.Format(" and t.engineerletdeptid='{0}' ", user.DeptId);
                }
                sql += string.Format(" and t.createuserorgcode like'%{0}%'", user.OrganizeCode);
            }
            else
                sql += string.Format(" and b.departmentid='{0}'", deptid);
            if (!string.IsNullOrEmpty(year))
            {
                string startTime = new DateTime(Convert.ToInt32(year), 1, 1).ToString();
                string endTime = new DateTime(Convert.ToInt32(year), 12, 31).ToString();
                sql += string.Format(" and (t.createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and t.createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(startTime).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(endTime).ToString("yyyy-MM-dd 23:59:59"));
            }

            sql += " group by d.itemvalue,d.itemname";
            DataTable dt = this.BaseRepository().FindTable(sql);

            int count = dt.Select("itemvalue='001'").Length == 0 ? 0 : int.Parse(dt.Select("itemvalue='001'")[0][0].ToString());
            object[] arr = { "长协外包", count };
            list.Add(arr);
            count = dt.Select("itemvalue='002'").Length == 0 ? 0 : int.Parse(dt.Select("itemvalue='002'")[0][0].ToString());
            arr = new object[] { "临时外包", count };
            list.Add(arr);
            dt.Dispose();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 根据部门获取工程类型数量统计图表数据
        /// </summary>
        /// <param name="deptid">部门id</param>
        /// <param name="year">统计年份</param>
        /// <param name="type">工程类型001 长协 002 外包</param>
        /// <returns></returns>
        public string GetTypeList(string deptid, string year = "", string type = "001,002")
        {
            List<OutEngineerStatEntity> list = new List<OutEngineerStatEntity>();
            Dictionary<string, int> dic = new Dictionary<string, int>();
            string sql = string.Format(@" select count(*) num, d.itemname,d.itemvalue
                                                  from epg_outsouringengineer t
                                                  left join (select m.itemname, m.itemvalue
                                                               from base_dataitem t
                                                               left join base_dataitemdetail m on m.itemid = t.itemid
                                                              where t.itemcode = 'ProjectType') d on d.itemvalue =t.engineertype
                                                  left join Base_Department b on b.departmentid = t.outprojectid where t.isdeptadd=1  ");
            if (string.IsNullOrEmpty(deptid))
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (user.RoleName.Contains("厂级部门用户") || user.RoleName.Contains("公司级用户"))
                {
                    //sql += string.Format(" where 1=1 ");
                }
                else
                {
                    sql += string.Format(" and t.engineerletdeptid='{0}' ", user.DeptId);
                }
                sql += string.Format(" and t.createuserorgcode like'%{0}%'", user.OrganizeCode);
            }
            else
                sql += string.Format(" and b.departmentid='{0}'", deptid);
            if (!string.IsNullOrEmpty(year))
            {
                string startTime = new DateTime(Convert.ToInt32(year), 1, 1).ToString();
                string endTime = new DateTime(Convert.ToInt32(year), 12, 31).ToString();
                sql += string.Format(" and (t.createdate>to_date('{0}','yyyy-mm-dd hh24:mi:ss') and t.createdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss'))", DateTime.Parse(startTime).ToString("yyyy-MM-dd 00:00:01"), DateTime.Parse(endTime).ToString("yyyy-MM-dd 23:59:59"));
            }
            sql += " group by d.itemvalue,d.itemname";
            DataTable dt = this.BaseRepository().FindTable(sql);

            int count1 = dt.Select("itemvalue='001'").Length == 0 ? 0 : int.Parse(dt.Select("itemvalue='001'")[0][0].ToString());
            int count2 = dt.Select("itemvalue='002'").Length == 0 ? 0 : int.Parse(dt.Select("itemvalue='002'")[0][0].ToString());

            int sum = count1 + count2;

            decimal percent1 = (sum == 0 ? 0 : decimal.Parse(count1.ToString()) / sum);
            percent1 = percent1 == 0 ? 0 : Math.Round(percent1 * 100, 2);
            decimal percent2 = (sum == 0 ? 0 : decimal.Parse(count2.ToString()) / sum);
            percent2 = percent2 == 0 ? 0 : Math.Round(percent2 * 100, 2);
            decimal percent3 = (sum == 0 ? 0 : decimal.Parse(sum.ToString()) / sum);
            percent3 = percent3 == 0 ? 0 : Math.Round(percent3 * 100, 2);
            OutEngineerStatEntity e1 = new OutEngineerStatEntity() { name = "长协外包", value = "001", num = count1, percent = percent1 };

            list.Add(e1);
            OutEngineerStatEntity e2 = new OutEngineerStatEntity() { name = "临时外包", value = "002", num = count2, percent = percent2 };

            list.Add(e2);
            OutEngineerStatEntity e3 = new OutEngineerStatEntity() { name = "工程总数", value = "", num = sum, percent = percent3 };

            list.Add(e3);

            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = list.Count,
                rows = list
            });
        }




        public DataTable GetEngineerByCurrDept()
        {
            Operator currUser = OperatorProvider.Provider.Current();
            string sql = string.Format(@"select t.engineername engineername,t.id engineerid,t.engineerletdeptid,t.engineerletpeopleid from epg_outsouringengineer t ");
            if (currUser.RoleName.Contains("省级"))
            {
                sql += string.Format(@" where t.createuserorgcode in(  select  encode
                                                from BASE_DEPARTMENT d
                                               where d.deptcode like '{0}%'
                                                 and d.nature = '厂级'
                                                 and d.description is null) ", currUser.NewDeptCode);
            }
            else if (currUser.IsSystem || currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户") || currUser.RoleName.Contains("公司管理员") || currUser.RoleName.Contains("公司领导"))
            {
                sql += string.Format(" where t.createuserorgcode like'%{0}%' ", currUser.OrganizeCode);
            }
            else if (currUser.RoleName.Contains("承包商级用户"))
            {
                sql += string.Format(@" where t.outprojectid ='{0}'  and t.createuserorgcode='{1}'", currUser.DeptId, currUser.OrganizeCode);
            }
            else if (currUser.RoleName.Contains("部门级用户"))
            {
                sql += string.Format(@" where t.engineerletdeptid ='{0}' and t.createuserorgcode='{1}'", currUser.DeptId, currUser.OrganizeCode);
            }
            //sql += " and t.isdeptadd=1 ";
            //sql += string.Format(@" and t.createuserorgcode like'%{0}%' ", currUser.OrganizeCode);
            return this.BaseRepository().FindTable(sql.ToString());
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
        public void SaveForm(string keyValue, OutsouringengineerEntity entity)
        {

            if (!string.IsNullOrEmpty(keyValue))
            {

                entity.Modify(keyValue);
                if (!string.IsNullOrEmpty(entity.ENGINEERUSEDEPT))
                    entity.ENGINEERUSEDEPT = new Repository<DepartmentEntity>(DbFactory.Base()).FindEntity(entity.ENGINEERUSEDEPTID).FullName;
                if (!string.IsNullOrEmpty(entity.ENGINEERLETDEPT))
                    entity.ENGINEERLETDEPT = new Repository<DepartmentEntity>(DbFactory.Base()).FindEntity(entity.ENGINEERLETDEPTID).FullName;

                entity.OUTPROJECTNAME = null;
                entity.ENGINEERAREANAME = null;
                entity.ENGINEERLEVELNAME = null;
                entity.ENGINEERTYPENAME = null;
                entity.OUTPROJECTCODE = null;
                entity.OUTPROJECTNAME = null;
                this.BaseRepository().Update(entity);
            }
            else
            {
                var res = DbFactory.Base().BeginTrans();
                try
                {
                    entity.Create();
                    string sql = string.Format("select * from EPG_OutSouringEngineer where createuserorgcode ='{0}'", entity.CREATEUSERORGCODE);
                    int Code = this.BaseRepository().FindList(sql).ToList().Count;
                    switch (Code.ToString().Length)
                    {
                        case 1:
                            entity.ENGINEERCODE = "HDC000" + (Code + 1);
                            break;
                        case 2:
                            entity.ENGINEERCODE = "HDC00" + (Code + 1);
                            break;
                        case 3:
                            entity.ENGINEERCODE = "HDC0" + (Code + 1);
                            break;
                        default:
                            entity.ENGINEERCODE = "HDC" + (Code + 1);
                            break;
                    }
                    if (!string.IsNullOrEmpty(entity.ENGINEERUSEDEPTID))
                        entity.ENGINEERUSEDEPT = new Repository<DepartmentEntity>(DbFactory.Base()).FindEntity(entity.ENGINEERUSEDEPTID).FullName;
                    if (!string.IsNullOrEmpty(entity.ENGINEERLETDEPTID))
                        entity.ENGINEERLETDEPT = new Repository<DepartmentEntity>(DbFactory.Base()).FindEntity(entity.ENGINEERLETDEPTID).FullName;
                    res.Insert<OutsouringengineerEntity>(entity);
                    //生成一条资质审查数据关联工程Id 以及初始化一条工程流程状态数据
                    //AptitudeinvestigateinfoEntity Aptitude = new AptitudeinvestigateinfoEntity();
                    //Aptitude.ID = Guid.NewGuid().ToString();
                    //Aptitude.OUTPROJECTID = entity.OUTPROJECTID;
                    //Aptitude.OUTENGINEERID = entity.ID;
                    //Aptitude.ISSAVEORCOMMIT = "0";
                    //res.Insert<AptitudeinvestigateinfoEntity>(Aptitude);
                    StartappprocessstatusEntity startProcess = new StartappprocessstatusEntity();
                    startProcess.ID = Guid.NewGuid().ToString();
                    startProcess.OUTPROJECTID = entity.OUTPROJECTID;
                    startProcess.OUTENGINEERID = entity.ID;
                    startProcess.EXAMSTATUS = "0";
                    startProcess.SECURITYSTATUS = "0";
                    startProcess.PACTSTATUS = "0";
                    startProcess.TECHNICALSTATUS = "0";
                    startProcess.THREETWOSTATUS = "0";
                    startProcess.EQUIPMENTTOOLSTATUS = "0";
                    startProcess.PEOPLESTATUS = "0";
                    res.Insert<StartappprocessstatusEntity>(startProcess);

                    res.Commit();
                }
                catch
                {
                    res.Rollback();
                }


            }
        }

        public bool ProIsOver(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var project = this.BaseRepository().FindEntity(keyValue);
                if (project != null)
                {
                    project.ENGINEERSTATE = "003";
                    project.ACTUALENDDATE = DateTime.Now;
                    this.BaseRepository().Update(project);
                    return true;
                }
            }
            return false;
        }
        #endregion



    }
}
