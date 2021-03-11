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
    /// �� �������������Ϣ��
    /// </summary>
    public class OutsouringengineerService : RepositoryFactory<OutsouringengineerEntity>, OutsouringengineerIService
    {
        private DepartmentService DepartmentService = new DepartmentService();
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //��ѯ����
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
                                                                    and nature='�а���' order by encode asc )t where rownum=1) ",
                                                                                         dept.EnCode);
                
                }
                else
                {
                    pagination.conditionJson += string.Format(" and e.outprojectid='{0}' ", queryParam["OutProjectId"].ToString());
                }
            }
            //����״̬
            if (!queryParam["engineerstate"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and e.engineerstate='{0}' ", queryParam["engineerstate"].ToString());
            }
            //����״̬ ��ѡ
            if (!queryParam["engineerstatIn"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and e.engineerstate in({0}) ", queryParam["engineerstatIn"].ToString());
            }
            //����ͳ����ת
            if (!queryParam["sTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and e.createdate>=to_date('{0}','yyyy-mm-dd') ", queryParam["sTime"].ToString());
            }
            if (!queryParam["eTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and e.createdate<=to_date('{0}','yyyy-mm-dd') ", Convert.ToDateTime(queryParam["eTime"]).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //ʡ��ͳ����תʹ��--����ɾ��
            if (!queryParam["fullName"].IsEmpty())
            {
                if (queryParam["fullName"].ToString() == "ȫ��")
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


            //������ת-����ɾ��
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
                        item["processState"] += item["pactstatus"].ToString() + "��";
                    if (!string.IsNullOrWhiteSpace(item["threetwostatus"].ToString()))
                        item["processState"] += item["threetwostatus"].ToString() + "��";
                    if (!string.IsNullOrWhiteSpace(item["compactstatus"].ToString()))
                        item["processState"] += item["compactstatus"].ToString() + "��";
                    if (!string.IsNullOrWhiteSpace(item["technicalstatus"].ToString()))
                        item["processState"] += item["technicalstatus"].ToString() + "��";
                    if (!string.IsNullOrWhiteSpace(item["equipmenttoolstatus"].ToString()))
                        item["processState"] += item["equipmenttoolstatus"].ToString() + "��";
                    if (!string.IsNullOrWhiteSpace(item["peoplestatus"].ToString()))
                        item["processState"] += item["peoplestatus"].ToString() + "��";

                    if (item["processState"].ToString().Length > 0)
                    {
                        item["processState"] = item["processState"].ToString().Substring(0, item["processState"].ToString().Length - 1);
                    }
                    if (item["processState"].ToString().Length == 0)
                    {
                        item["processState"] = "��������";
                    }
                }
                if (item["engineerstate"].ToString() == "�ڽ�")
                {
                    item["processState"] = "������";
                }
                if (item["engineerstate"].ToString() == "���깤")
                {
                    item["processState"] = "���깤";
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
                    case "1"://��֤��
                        pagination.conditionJson += string.Format(" and e.id not in (select distinct(m.projectid) from epg_safetyeamestmoney m)");
                        break;
                    case "2"://��ͬ
                        pagination.conditionJson += string.Format(" and e.id not in (select distinct(m.projectid) from epg_compact m)");
                        break;
                    case "3"://Э��
                        pagination.conditionJson += string.Format(" and e.id not in (select distinct(m.projectid) from epg_protocol m)");
                        break;
                    case "4"://��ȫ��������
                        pagination.conditionJson += string.Format("  and e.id not in (select distinct(m.projectid) from epg_techdisclosure m) ");
                        break;
                    default:
                        break;
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);

        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OutsouringengineerEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OutsouringengineerEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OutsouringengineerEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ���ݵ�ǰ��½�˺�������Ϣ��ѯ������Ϣ
        /// </summary>
        /// <param name="currUser">��ǰ�û�</param>
        /// <param name="mode">001--��λ���� 002--��Ա���� 003--��ͬ 004--Э�� 005--��ȫ�������� 006--�������� 007--����������  008--�볡��� 009--�������� 010--��֤�� </param>
        /// <param name="orgid">�糧ID</param>
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
                string allrangedept = new DataItemDetailService().GetItemValue("�豸����", "SBDept");
                if (currUser.RoleName.Contains("ʡ���û�"))
                {
                    string tWhere = " 1=1";
                    if (!orgid.IsEmpty())
                    {
                        tWhere += string.Format(" and d.organizeid ='{0}'", orgid);
                    }
                    strSql.AppendFormat(@" and e.createuserorgcode in (select encode from base_department d where d.deptcode like '{0}%' and d.nature='����' and d.description is null and {1})", currUser.NewDeptCode, tWhere);
                }
                else if (currUser.RoleName.Contains("���������û�") || currUser.RoleName.Contains("��˾���û�") || currUser.RoleName.Contains("��˾����Ա") || currUser.RoleName.Contains("��˾�쵼") || currUser.DeptId== allrangedept)
                {
                    strSql.Append(string.Format(" and e.createuserorgcode like'%{0}%' ", currUser.OrganizeCode));
                }
                else if (currUser.RoleName.Contains("�а��̼��û�"))
                {
                    strSql.Append(string.Format(" and (e.outprojectid ='{0}' or e.SUPERVISORID='{0}')", currUser.DeptId));
                }
                else
                {
                    var deptentity = DepartmentService.GetEntity(currUser.DeptId);
                    while (deptentity.Nature == "����" || deptentity.Nature == "רҵ")
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
                    strSql.Append(string.Format(" and e.id not in(select outengineerid from epg_startapplyfor��  "));
                }
                if (mode == "012")
                {
                    strSql.Append(string.Format(" and e.id not in(select projectid from epg_safetyevaluate��and e.engineerstate='002' "));
                }
                if (mode == "013") // ��ȫ�������ۼƻ���ѯ�ڽ�������� ���� 2020/10/14
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
                var config = configService.GetEntityByModuleCode(data.Rows[i]["engineerid"].ToString(), mode); //������Ŀ��ѯÿһ����Ŀ������������
                if (config == null)
                {
                    config = configService.GetEntityByModuleCode(currUser.OrganizeId, mode); //��ʷ������û�и�����Ŀȥ�����̣����Ը��ݵ糧idȥ��ѯĬ������
                }
                if (config != null)
                {
                    if (!string.IsNullOrWhiteSpace(config.FrontModuleCode))
                    {
                        var listConfig = config.FrontModuleCode.Split(',');
                        var expression = LinqExtensions.True<StartappprocessstatusEntity>();
                        for (int j = 0; j < listConfig.Length; j++) //���������������ѯ���������ת���ѯ����
                        {
                            switch (listConfig[j])
                            {
                                case "001"://��λ����
                                    expression = expression.And(t => t.EXAMSTATUS == "1");
                                    break;
                                case "002"://��Ա����
                                    expression = expression.And(t => t.PEOPLESTATUS == "1");
                                    break;
                                case "003"://��ͬ
                                    expression = expression.And(t => t.COMPACTSTATUS == "1");
                                    break;
                                case "004"://Э��
                                    expression = expression.And(t => t.PACTSTATUS == "1");
                                    break;
                                case "005"://��ȫ��������
                                    expression = expression.And(t => t.TECHNICALSTATUS == "1");
                                    break;
                                case "006"://��������
                                    expression = expression.And(t => t.THREETWOSTATUS == "1");
                                    break;
                                case "007"://������
                                    expression = expression.And(t => t.EQUIPMENTTOOLSTATUS == "1");
                                    break;
                                case "008"://�볡���
                                    expression = expression.And(t => t.SPTOOLSSTATUS == "1");
                                    break;
                                case "009"://��������
                                    break;
                                case "010"://��֤��
                                    expression = expression.And(t => t.SECURITYSTATUS == "1");
                                    break;
                                default:
                                    break;
                            }
                        }
                        string OUTENGINEERID = data.Rows[i]["engineerid"].ToString();
                        expression = expression.And(t => t.OUTENGINEERID == OUTENGINEERID);
                        if (DbFactory.Base().FindEntity<StartappprocessstatusEntity>(expression) == null) //�������Ŀ������ת���������� ���Ƴ�������Ŀ
                        {
                            data.Rows.RemoveAt(i);
                        }
                    }
                }
            }
            return data;
        }




        /// <summary>
        /// ���ݵ�ǰ��½�˺�������Ϣ��ѯ������Ϣ
        /// </summary>
        /// <param name="currUser">��ǰ�û�</param>
        /// <param name="mode">001--��λ���� 002--��Ա���� 003--��ͬ 004--Э�� 005--��ȫ�������� 006--�������� 007--����������  008--�볡��� 009--�������� 010--��֤�� </param>
        /// <param name="orgid">�糧ID</param>
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
                                            decode(ss.examstatus, '1', '', '��λ�������') examstatus,
                                           decode(ss.pactstatus, '1', '', 'Э�����') pactstatus,
                                           decode(ss.compactstatus, '1', '', '��ͬ����') compactstatus,
                                           decode(ss.threetwostatus, '1', '', '��������') threetwostatus,
                                           decode(ss.technicalstatus, '1', '', '��ȫ��������') technicalstatus,
                                           decode(ss.equipmenttoolstatus, '1', '', '����������') equipmenttoolstatus,
                                            decode(ss.peoplestatus, '1', '', '��Ա�������') peoplestatus,
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
                else if (currUser.RoleName.Contains("ʡ��"))
                {
                    strSql.Append(string.Format(@" e.createuserorgcode  in (select encode
                        from base_department d
                        where d.deptcode like '{0}%' and d.nature = '����' and d.description is null)", currUser.NewDeptCode));
                }
                else if (currUser.RoleName.Contains("���������û�") || currUser.RoleName.Contains("��˾���û�"))
                {
                    strSql.Append(string.Format(" e.createuserorgcode like'%{0}%' ", currUser.OrganizeCode));
                }
                else if (currUser.RoleName.Contains("�а��̼��û�"))
                {
                    strSql.Append(string.Format("  e.outprojectid ='{0}'", currUser.DeptId));
                }
                else if (currUser.RoleName.Contains("רҵ���û�") || currUser.RoleName.Contains("���鼶�û�"))
                {
                    var pDept = new DepartmentService().GetParentDeptBySpecialArgs(currUser.ParentId, "����");
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
                                case "001"://��λ����
                                    strSql.Append(" and ss.examstatus='1'");
                                    break;
                                case "002"://��Ա����
                                    strSql.Append(" and ss.peoplestatus='1'");
                                    break;
                                case "003"://��ͬ
                                    strSql.Append(" and ss.compactstatus='1'");
                                    break;
                                case "004"://Э��
                                    strSql.Append(" and ss.pactstatus='1'");
                                    break;
                                case "005"://��ȫ��������
                                    strSql.Append(" and ss.technicalstatus='1'");
                                    break;
                                case "006"://��������
                                    strSql.Append(" and ss.threetwostatus='1'");
                                    break;
                                case "007"://������
                                    strSql.Append(" and ss.equipmenttoolstatus='1'");
                                    break;
                                case "008"://�볡���
                                    //strSql.Append(" and p.sptoolsstatus='1'");
                                    break;
                                case "009"://��������
                                    break;
                                case "010"://��֤��
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
                    strSql.Append(string.Format(" and e.id not in(select outengineerid from epg_startapplyfor��  "));
                }
                if (mode == "012")
                {
                    strSql.Append(string.Format(" and e.id not in(select projectid from epg_safetyevaluate��and e.engineerstate='002' "));
                }
                //�ڽ�״̬
                if (mode == "100")
                {
                    strSql.Append(string.Format(" and e.engineerstate='002' "));
                }
            }

            return this.BaseRepository().FindTable(strSql.ToString());
        }

        /// <summary>
        /// ��ȡδͣ���򸴹����ͨ�����ڽ�������Ϣ
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
            else if (currUser.RoleName.Contains("���������û�") || currUser.RoleName.Contains("��˾���û�") || currUser.RoleName.Contains("��˾����Ա") || currUser.RoleName.Contains("��˾�쵼"))
            {
                strSql.Append(string.Format(" and e.createuserorgcode like'%{0}%' ", currUser.OrganizeCode));
            }
            else if (currUser.RoleName.Contains("�а��̼��û�"))
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
        /// �������ID��ȡδͣ�����ڽ�������Ϣ
        /// t.stopreturnstate!='1' δͣ���Ĺ���
        /// e.engineerstate='002' ����������ͨ�����
        /// </summary>
        /// <param name="deptId">�����λID</param>
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
        /// ��ȡ���̵�����״̬ͼ
        /// </summary>
        /// <param name="keyValue">����Id</param>
        /// <returns></returns>
        public Flow GetProjectFlow(string keyValue)
        {

            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            var isendflow = false;//���̽������
            Flow flow = new Flow();
            flow.title = "";
            flow.initNum = 22;
            Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var project = this.BaseRepository().FindEntity(keyValue);
            //����״̬Ϊ�ѿ��������깤�����꿪������������̽���
            if (project.ENGINEERSTATE == "003")
            {
                isendflow = true;
            }
            int lastleft = 0;
            int lasttop = 0;
            string firstId = string.Empty;//��һ���ڵ�Id
            string lastId = string.Empty;//���һ���ڵ�Id
            string startId = string.Empty;//���̿�ʼ�ڵ�
            string endId = string.Empty;//���̽����ڵ�Id
            string firstCode = string.Empty;//��һ���ڵ�Code
            string lastCode = "011";//���ý����ڵ�(���̽�����ǰһ���ڵ�)
            if (project != null)
            {
                //��ѯ�糧�������������
                string sql = string.Format(@"select t.id,t.modulename,t.modulecode,t.frontmodulecode,t.frontmodulename,t.deptcode,t.address from wf_schemecontent a left join epg_outprocessconfig t on a.id=t.recid where a.wfschemeinfoid='{0}'  order by t.modulecode ", keyValue);
                DataTable dt = this.BaseRepository().FindTable(sql);
                if (dt.Rows.Count == 0) //������ݹ���û�в鵽������̣������Ǿ����ݣ���ʹ�þ��߼� ���ݵ�λ��ѯ����
                {
                    sql = string.Format(@"select t.id,t.modulename,t.modulecode,t.frontmodulecode,t.frontmodulename,t.deptcode,t.address from wf_schemecontent a left join epg_outprocessconfig t on a.id=t.recid where a.wfschemeinfoid='{0}'  order by t.modulecode ", new DepartmentService().GetEntityByCode(project.CREATEUSERORGCODE).DepartmentId);
                    dt = this.BaseRepository().FindTable(sql);
                }
                //ȷ�ϵ糧��һ�����̺Ϳ�����������
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulecode"].ToString()))
                    {
                        firstCode = dt.Rows[i]["modulecode"].ToString();
                    }
                }
                //������������ѯ����״̬(���ﲻ��Ҫ�ж�,ǰ���ж���û�����������ǲ��ܽ����ģ���)
                string sqlStatus = string.Format(@"select t.examstatus,t.securitystatus,t.pactstatus,t.technicalstatus,t.threetwostatus,
                                            t.equipmenttoolstatus,t.peoplestatus,t.compactstatus,t.sptoolsstatus from EPG_STARTAPPPROCESSSTATUS t where t.outengineerid='{0}'", project.ID);
                DataTable dtStatus = this.BaseRepository().FindTable(sqlStatus);

                //���̿�ʼ�ڵ�
                nodes nodes_start = new nodes();
                nodes_start.alt = true;
                nodes_start.isclick = false;
                nodes_start.css = "";
                nodes_start.id = Guid.NewGuid().ToString();
                nodes_start.img = "";
                nodes_start.name = "���̿�ʼ";
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
                        nodes.id = dr["id"].ToString(); //����
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
                            #region �������״̬(0:δ���1:���)
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
                                            nodedesignatedata.status = item.AUDITRESULT.ToString() == "0" ? "ͨ��" : "��ͨ��";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
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
                                            nodedesignatedata.status = "�����";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }
                                    }
                                    else
                                    {
                                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                        nodedesignatedata.createdate = "��";
                                        nodedesignatedata.creatdept = "";
                                        nodedesignatedata.createuser = "";
                                        nodedesignatedata.status = "��";
                                        nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                        nodelist.Add(nodedesignatedata);
                                    }
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }
                            #endregion
                            #region ��ȫ��֤��״̬(0:δ���1:���)
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
                                            nodedesignatedata.status = item.AUDITRESULT.ToString() == "0" ? "ͨ��" : "��ͨ��";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
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
                                    nodedesignatedata.createdate = "��";
                                    nodedesignatedata.creatdept = "";
                                    nodedesignatedata.createuser = "";
                                    nodedesignatedata.status = "��";
                                    nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                    nodelist.Add(nodedesignatedata);
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }
                            #endregion
                            #region Э��״̬(0:δ���1:���)
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
                                        nodedesignatedata.status = "ͨ��";
                                        nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                        nodelist.Add(nodedesignatedata);
                                    }

                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                                else
                                {
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                    nodedesignatedata.createdate = "��";
                                    nodedesignatedata.creatdept = "��";
                                    nodedesignatedata.createuser = "��";
                                    nodedesignatedata.status = "��";
                                    nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                    nodelist.Add(nodedesignatedata);
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }
                            #endregion
                            #region ��ȫ��������״̬(0:δ���1:���)
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
                                            nodedesignatedata.status = "ͨ��";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
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
                                    nodedesignatedata.createdate = "��";
                                    nodedesignatedata.creatdept = "";
                                    nodedesignatedata.createuser = "";
                                    nodedesignatedata.status = "��";
                                    nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                    nodelist.Add(nodedesignatedata);
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }
                            #endregion
                            #region ��������״̬(0:δ���1:���)
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
                                                nodedesignatedata.status = item.AUDITRESULT.ToString() == "0" ? "ͨ��" : "��ͨ��";
                                                nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
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
                                            nodedesignatedata.status = "�����";
                                            //nodedesignatedata.nextuser = itInfo.FLOWDEPTNAME + itInfo.FLOWROLENAME;
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }
                                    }
                                    else
                                    {
                                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                        nodedesignatedata.createdate = "��";
                                        nodedesignatedata.creatdept = "";
                                        nodedesignatedata.createuser = "";
                                        nodedesignatedata.status = "��";
                                        nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                        nodelist.Add(nodedesignatedata);
                                    }
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }
                            #endregion
                            #region �豸����������״̬(0:δ���1:���)
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
                                            nodedesignatedata.status = item.AUDITRESULT.ToString() == "0" ? "ͨ��" : "��ͨ��";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
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
                                            nodedesignatedata.status = "�����";
                                            //nodedesignatedata.nextuser = itInfo.FLOWDEPTNAME + itInfo.FLOWROLENAME;
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }

                                    }
                                    else
                                    {
                                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                        nodedesignatedata.createdate = "��";
                                        nodedesignatedata.creatdept = "";
                                        nodedesignatedata.createuser = "";
                                        nodedesignatedata.status = "��";
                                        nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                        nodelist.Add(nodedesignatedata);
                                    }
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }

                            #endregion
                            #region ��Ա���״̬(0:δ���1:���)
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
                                            nodedesignatedata.status = item.AUDITRESULT.ToString() == "0" ? "ͨ��" : "��ͨ��";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
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
                                            nodedesignatedata.createdate = "��";
                                            nodedesignatedata.creatdept = new DepartmentService().GetList().Where(x => x.EnCode == itInfo.CREATEUSERDEPTCODE).FirstOrDefault().FullName;
                                            nodedesignatedata.createuser = itInfo.CREATEUSERNAME;
                                            nodedesignatedata.status = "��";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }
                                    }
                                    else
                                    {
                                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                        nodedesignatedata.createdate = "��";
                                        nodedesignatedata.creatdept = "";
                                        nodedesignatedata.createuser = "";
                                        nodedesignatedata.status = "��";
                                        nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                        nodelist.Add(nodedesignatedata);
                                    }
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }
                            #endregion
                            #region ��ͬ״̬(0:δ���1:���)
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
                                        nodedesignatedata.status = "ͨ��";
                                        nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                        nodelist.Add(nodedesignatedata);
                                    }

                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                                else
                                {
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                    nodedesignatedata.createdate = "��";
                                    nodedesignatedata.creatdept = "��";
                                    nodedesignatedata.createuser = "��";
                                    nodedesignatedata.status = "��";
                                    nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                    nodelist.Add(nodedesignatedata);
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }
                            #endregion
                            #region �볧���
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
                                            nodedesignatedata.status = "ͨ��";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
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
                                            nodedesignatedata.status = item.INVESTIGATESTATE == "1" ? "�����" : "�����";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }
                                    }
                                    else
                                    {
                                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                        nodedesignatedata.createdate = "��";
                                        nodedesignatedata.creatdept = "";
                                        nodedesignatedata.createuser = "";
                                        nodedesignatedata.status = "��";
                                        nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                        nodelist.Add(nodedesignatedata);
                                    }
                                }
                                sinfo.NodeDesignateData = nodelist;
                                nodes.setInfo = sinfo;
                            }
                            #endregion
                            #region ��������
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
                                            nodedesignatedata.status = "ͨ��";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
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
                                            nodedesignatedata.status = item.ISINVESTOVER == 1 ? "�����" : "�����";
                                            nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                            nodelist.Add(nodedesignatedata);
                                        }
                                    }
                                    else
                                    {
                                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                        nodedesignatedata.createdate = "��";
                                        nodedesignatedata.creatdept = "";
                                        nodedesignatedata.createuser = "";
                                        nodedesignatedata.status = "��";
                                        nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                        nodelist.Add(nodedesignatedata);
                                    }
                                }
                                sinfo.NodeDesignateData = nodelist;
                                nodes.setInfo = sinfo;
                            }
                            #endregion
                            #region ��ȫ����
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
                                    nodedesignatedata.status = "ͨ��" ;
                                    nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                    nodelist.Add(nodedesignatedata);
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                                else
                                {
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                    nodedesignatedata.createdate = "��";
                                    nodedesignatedata.creatdept = "";
                                    nodedesignatedata.createuser = "";
                                    nodedesignatedata.status = "��";
                                    nodedesignatedata.prevnode = string.IsNullOrWhiteSpace(dt.Rows[i]["frontmodulename"].ToString()) ? "��" : dt.Rows[i]["frontmodulename"].ToString();
                                    nodelist.Add(nodedesignatedata);
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }
                            #endregion
                        }
                        nlist.Add(nodes);
                    }
                    //���̽����ڵ�
                    nodes nodes_end = new nodes();
                    nodes_end.alt = true;
                    nodes_end.isclick = false;
                    nodes_end.css = "";
                    nodes_end.id = Guid.NewGuid().ToString();
                    nodes_end.img = "";
                    nodes_end.name = "���̽���";
                    nodes_end.type = "endround";
                    nodes_end.width = 150;
                    nodes_end.height = 60;
                    //ȡ���һ���̵�λ�ã������λ
                    nodes_end.left = lastleft;
                    nodes_end.top = lasttop + 100;
                    endId = nodes_end.id;
                    nlist.Add(nodes_end);

                    //���״̬Ϊ���ͨ����ͨ�������̽������б�ʶ 
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
        /// ���ݵ�ǰ��¼�� 
        /// ��ȡ�Ѿ�ͣ���Ĺ�����Ϣ
        /// ��ȡ��û����Ӹ����Ĺ���
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
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
            else if (currUser.RoleName.Contains("���������û�") || currUser.RoleName.Contains("��˾���û�") || currUser.RoleName.Contains("��˾����Ա") || currUser.RoleName.Contains("��˾�쵼"))
            {
                strSql.Append(string.Format(" where e.createuserorgcode like'%{0}%' ", currUser.OrganizeCode));
            }
            else if (currUser.RoleName.Contains("�а��̼��û�"))
            {
                strSql.Append(string.Format(" where e.outprojectid ='{0}'", currUser.DeptId));
            }
            else
            {
                strSql.Append(string.Format(" where e.engineerletdeptid='{0}' ", currUser.DeptId));
            }
            strSql.Append(@"  and e.stopreturnstate='1' and e.id not in (select outengineerid from epg_returntowork  r
left join epg_aptitudeinvestigateaudit a on r.id=a.aptitudeid where a.auditresult='1' or a.auditresult is null) ");//ͣ���Ĺ��̻��Ѿ���Ӹ����Ĺ���
            return this.BaseRepository().FindTable(strSql.ToString());
        }
        /// <summary>
        /// �������״̬ͳ��ͼ
        /// </summary>
        /// <param name="deptid">����Id</param>
        /// <param name="year">ʱ��</param>
        /// <param name="state">״ֵ̬001 δ���� 002 �ڽ� 003 ���깤</param>
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
                if (user.RoleName.Contains("���������û�") || user.RoleName.Contains("��˾���û�"))
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
            object[] arr = { "δ����", count };
            list.Add(arr);
            count = dt.Select("itemvalue='002'").Length == 0 ? 0 : int.Parse(dt.Select("itemvalue='002'")[0][0].ToString());
            arr = new object[] { "�ڽ�", count };
            list.Add(arr);
            count = dt.Select("itemvalue='003'").Length == 0 ? 0 : int.Parse(dt.Select("itemvalue='003'")[0][0].ToString());
            arr = new object[] { "���깤", count };
            list.Add(arr);
            dt.Dispose();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// �������״̬ͳ�Ʊ�
        /// </summary>
        /// <param name="deptid">����Id</param>
        /// <param name="year">ʱ��</param>
        /// <param name="state">״ֵ̬001 δ���� 002 �ڽ� 003 ���깤</param>
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
                if (user.RoleName.Contains("���������û�") || user.RoleName.Contains("��˾���û�"))
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

            OutEngineerStatEntity e1 = new OutEngineerStatEntity() { name = "δ����", value = "001", num = count1, percent = percent1 };
            list.Add(e1);

            OutEngineerStatEntity e2 = new OutEngineerStatEntity() { name = "�ڽ�", value = "002", num = count2, percent = percent2 };
            list.Add(e2);

            OutEngineerStatEntity e3 = new OutEngineerStatEntity() { name = "���깤", value = "003", num = count3, percent = percent3 };
            list.Add(e3);

            OutEngineerStatEntity e4 = new OutEngineerStatEntity() { name = "��������", value = "", num = sum, percent = percent4 };
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
        /// ���ݲ��Ż�ȡ������������ͳ��ͼ������
        /// </summary>
        /// <param name="deptid">����id</param>
        /// <param name="year">ͳ�����</param>
        /// <param name="type">��������001 ��Э 002 ���</param>
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
                if (user.RoleName.Contains("���������û�") || user.RoleName.Contains("��˾���û�"))
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
            object[] arr = { "��Э���", count };
            list.Add(arr);
            count = dt.Select("itemvalue='002'").Length == 0 ? 0 : int.Parse(dt.Select("itemvalue='002'")[0][0].ToString());
            arr = new object[] { "��ʱ���", count };
            list.Add(arr);
            dt.Dispose();
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// ���ݲ��Ż�ȡ������������ͳ��ͼ������
        /// </summary>
        /// <param name="deptid">����id</param>
        /// <param name="year">ͳ�����</param>
        /// <param name="type">��������001 ��Э 002 ���</param>
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
                if (user.RoleName.Contains("���������û�") || user.RoleName.Contains("��˾���û�"))
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
            OutEngineerStatEntity e1 = new OutEngineerStatEntity() { name = "��Э���", value = "001", num = count1, percent = percent1 };

            list.Add(e1);
            OutEngineerStatEntity e2 = new OutEngineerStatEntity() { name = "��ʱ���", value = "002", num = count2, percent = percent2 };

            list.Add(e2);
            OutEngineerStatEntity e3 = new OutEngineerStatEntity() { name = "��������", value = "", num = sum, percent = percent3 };

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
            if (currUser.RoleName.Contains("ʡ��"))
            {
                sql += string.Format(@" where t.createuserorgcode in(  select  encode
                                                from BASE_DEPARTMENT d
                                               where d.deptcode like '{0}%'
                                                 and d.nature = '����'
                                                 and d.description is null) ", currUser.NewDeptCode);
            }
            else if (currUser.IsSystem || currUser.RoleName.Contains("���������û�") || currUser.RoleName.Contains("��˾���û�") || currUser.RoleName.Contains("��˾����Ա") || currUser.RoleName.Contains("��˾�쵼"))
            {
                sql += string.Format(" where t.createuserorgcode like'%{0}%' ", currUser.OrganizeCode);
            }
            else if (currUser.RoleName.Contains("�а��̼��û�"))
            {
                sql += string.Format(@" where t.outprojectid ='{0}'  and t.createuserorgcode='{1}'", currUser.DeptId, currUser.OrganizeCode);
            }
            else if (currUser.RoleName.Contains("���ż��û�"))
            {
                sql += string.Format(@" where t.engineerletdeptid ='{0}' and t.createuserorgcode='{1}'", currUser.DeptId, currUser.OrganizeCode);
            }
            //sql += " and t.isdeptadd=1 ";
            //sql += string.Format(@" and t.createuserorgcode like'%{0}%' ", currUser.OrganizeCode);
            return this.BaseRepository().FindTable(sql.ToString());
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
                    //����һ������������ݹ�������Id �Լ���ʼ��һ����������״̬����
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
