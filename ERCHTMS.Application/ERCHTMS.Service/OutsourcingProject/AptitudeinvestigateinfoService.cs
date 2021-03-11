using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using System;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.PublicInfoManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity;
using ERCHTMS.Service.HighRiskWork;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.Service.PowerPlantInside;
using ERCHTMS.Service.PersonManage;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Service.RoutineSafetyWork;
using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.Service.SystemManage;
using System.Data.Common;

using ERCHTMS.Service.EquipmentManage;
using ERCHTMS.Entity.EquipmentManage;


namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// �� ����������������Ϣ��
    /// </summary>
    public class AptitudeinvestigateinfoService : RepositoryFactory<AptitudeinvestigateinfoEntity>, AptitudeinvestigateinfoIService
    {
        private PeopleReviewIService review = new PeopleReviewService();
        private DataItemDetailService dataitemdetailbll = new DataItemDetailService();
        private OutsouringengineerService outsouringengineerservice = new OutsouringengineerService();
        private DepartmentService departmentservice = new DepartmentService();
        private ManyPowerCheckService manypowercheckservice = new ManyPowerCheckService();
        private UserService userservice = new UserService();
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            Operator currUser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["name"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (e.engineername like'%{0}%' or b.fullname like'%{1}%') ", queryParam["name"].ToString(), queryParam["name"].ToString());
            }
            if (!queryParam["orgCode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.createuserorgcode='{0}' ", queryParam["orgCode"].ToString());
            }
            if (!queryParam["StartTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.createdate>=to_date('{0}','yyyy-MM-dd hh24:mi:ss') ", queryParam["StartTime"].ToString());
            }
            if (!queryParam["EndTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.createdate<=to_date('{0}','yyyy-MM-dd hh24:mi:ss') ", Convert.ToDateTime(queryParam["EndTime"]).AddDays(1).ToString("yyyy-MM-dd"));
            }
            if (!queryParam["outengineerid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.outengineerid ='{0}' ", queryParam["outengineerid"].ToString());
            }
            if (!queryParam["indexState"].IsEmpty())//��ҳ����
            {
                string strCondition = "";
                strCondition = string.Format(" and t.createuserorgcode='{0}' and t.isauditover=0 and t.issaveorcommit='1'", currUser.OrganizeCode);
                DataTable data = BaseRepository().FindTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson + strCondition);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var engineerEntity = outsouringengineerservice.GetEntity(data.Rows[i]["outengineerid"].ToString());
                    var excutdept = departmentservice.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                    var outengineerdept = departmentservice.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                    var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentservice.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                    //��ȡ��һ�������
                    string str = manypowercheckservice.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, data.Rows[i]["outengineerid"].ToString());
                    data.Rows[i]["approveuserids"] = str;
                }

                string[] applyids = data.Select(" approveuserids like '%" + currUser.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();

                pagination.conditionJson += string.Format(" and t.id in ('{0}') {1}", string.Join("','", applyids), strCondition);
                
            }
            if (!queryParam["projectid"].IsEmpty())//���̹�������ͼ��ת
            {
                pagination.conditionJson += string.Format(" and e.id ='{0}'", queryParam["projectid"].ToString());
            }
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            dt.Columns.Add("auditpeople", typeof(string));
            dt.Columns.Add("auditresult", typeof(string));
            dt.Columns.Add("flowdeptname", typeof(string));
            dt.Columns.Add("flowname", typeof(string));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (item["isauditover"].ToString() == "1")
                    {
                        List<AptitudeinvestigateauditEntity> list = new AptitudeinvestigateauditService().GetList("").Where(x => x.APTITUDEID == item["id"].ToString()).ToList();
                        if (list.Count > 0)
                        {
                            AptitudeinvestigateauditEntity lastAudit = list.OrderBy(x => x.AUDITTIME).Last();
                            if (lastAudit != null)
                            {
                                item["auditpeople"] = lastAudit.AUDITPEOPLE;
                                item["auditresult"] = lastAudit.AUDITRESULT == "0" ? "�ϸ�" : "���ϸ�";
                            }
                        }
                        item["flowdeptname"] = "";
                        item["flowname"] = "";
                    }
                    else
                    {
                        var dept = new DepartmentService().GetEntity(item["nextcheckdeptid"].ToString());
                        item["flowdeptname"] = dept == null ? "" : dept.FullName;
                        item["flowname"] = dept == null ? "" : dept.FullName + "�����";
                    }

                }
            }
            return dt;
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<AptitudeinvestigateinfoEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public AptitudeinvestigateinfoEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ��������ID��ȡ������Ϣ
        /// </summary>
        /// <param name="engineerid"></param>
        /// <returns></returns>
        public AptitudeinvestigateinfoEntity GetEntityByOutEngineerId(string engineerid)
        {
            return this.BaseRepository().FindList(string.Format("select * from epg_aptitudeinvestigateinfo t where t.outengineerid='{0}' and  t.isauditover=1", engineerid)).OrderByDescending(x => x.CREATEDATE).ToList().FirstOrDefault();
        }
        /// <summary>
        /// ���������λID��ȡ���һ�����ͨ����λ������Ϣ
        /// </summary>
        /// <param name="outprojectId">�����λId</param>
        /// <returns></returns>
        public AptitudeinvestigateinfoEntity GetListByOutprojectId(string outprojectId)
        {
            string sql = string.Format(@"select *
                                            from epg_aptitudeinvestigateinfo t
                                            where t.outprojectid='{0}' 
                                            and  t.isauditover=1
                                            order by createdate desc", outprojectId);
            return this.BaseRepository().FindList(sql).ToList().FirstOrDefault();
        }

        /// <summary>
        /// �����������ID��ȡ���һ�����ͨ���ĵ�λ������Ϣ
        /// </summary>
        /// <param name="OutengineerId"></param>
        /// <returns></returns>
        public AptitudeinvestigateinfoEntity GetListByOutengineerId(string OutengineerId)
        {
            string sql = string.Format(@"select *
                                            from epg_aptitudeinvestigateinfo t
                                            where t.outengineerid='{0}'
                                            order by createdate desc", OutengineerId);
            return this.BaseRepository().FindList(sql).ToList().FirstOrDefault();
        }
        /// <summary>
        /// ��ѯ�������ͼ
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="urltype">��ѯ���ͣ�1 ��λ���� 2 ��Ա���� 3 �����豸���� 4 �綯/��ȫ���������� 5�������� 6�볧��� 7��������</param>
        /// <returns></returns>
        public Flow GetAuditFlowData(string keyValue, string urltype)
        {
            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            var isendflow = false;//���̽������
            string flowid = string.Empty;
            Flow flow = new Flow();
            flow.title = "";
            flow.initNum = 22;
            var currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string moduleno = string.Empty;
            var until = new BaseEntity();//����ʵ��
            OutsouringengineerEntity project = new OutsouringengineerEntity();//����ʵ��
            var projectService = new OutsouringengineerService();
            string table = string.Empty;
            string kbengineerletdeptid = string.Empty; // ����ʲ���̲���id
            var deptentityNew = new DepartmentEntity();
            string orgCode = currUser.OrganizeCode;
            switch (urltype)
            {
                case "1":
                    moduleno = "DWZZ";
                    until = this.BaseRepository().FindEntity(keyValue);
                    if ((until as AptitudeinvestigateinfoEntity).ISAUDITOVER == "1")
                    {
                        isendflow = true;
                    }
                    project = projectService.GetEntity((until as AptitudeinvestigateinfoEntity).OUTENGINEERID);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    flowid = (until as AptitudeinvestigateinfoEntity).FlowId;
                    table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' ",keyValue);
                    break;
                case "2":
                    moduleno = "RYZZ";
                    until = new PeopleReviewService().GetEntity(keyValue);
                    if ((until as PeopleReviewEntity).ISAUDITOVER == "1")
                    {
                        isendflow = true;
                    }
                    project = projectService.GetEntity((until as PeopleReviewEntity).OUTENGINEERID);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    flowid = (until as PeopleReviewEntity).FlowId;
                    table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' ", keyValue);
                    break;
                case "3":
                    moduleno = "TZSBGQJ";
                    until = new ToolsService().GetEntity(keyValue);
                    if ((until as ToolsEntity).ISOVER == "1")
                    {
                        isendflow = true;
                    }
                    project = projectService.GetEntity((until as ToolsEntity).OUTENGINEERID);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    //workdeptid = (until as ToolsEntity).FLOWDEPT;
                    flowid = (until as ToolsEntity).FlowId;
                    table = string.Format(@"left join epg_toolsaudit b on t.id = b.flowid and b.toolsid = '{0}' ", keyValue);
                    break;
                case "4":
                    moduleno = "SBGQJ";
                    until = new ToolsService().GetEntity(keyValue);
                    if ((until as ToolsEntity).ISOVER == "1")
                    {
                        isendflow = true;
                    }
                    project = projectService.GetEntity((until as ToolsEntity).OUTENGINEERID);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    flowid = (until as ToolsEntity).FlowId;
                    table = string.Format(@"left join epg_toolsaudit b on t.id = b.flowid and b.toolsid = '{0}' ", keyValue);
                    break;
                case "5":
                    until = new SchemeMeasureService().GetEntity(keyValue);
                    if ((until as SchemeMeasureEntity).ISOVER == "1")
                    {
                        isendflow = true;
                    }
                    project = projectService.GetEntity((until as SchemeMeasureEntity).PROJECTID);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    if (project == null) // �����������ʱ��
                    {
                        moduleno = "SCLA_SSGC";
                        kbengineerletdeptid = (until as SchemeMeasureEntity).ENGINEERLETDEPTID;
                    }
                    else
                    {
                        if (dataitemdetailbll.GetDataItemListByItemCode("FlowWithRiskLevel").Count() > 0)
                        {
                            moduleno = "SCLA" + project.ENGINEERLEVEL;
                        }
                        else
                        {
                            moduleno = "SCLA";
                        }
                    }
                    
                       
                    flowid = (until as SchemeMeasureEntity).FlowId;
                    table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' ", keyValue);
                    break;
                case "6":
                    moduleno = "RCXKSC";
                    until = new IntromissionService().GetEntity(keyValue);
                    if ((until as IntromissionEntity).INVESTIGATESTATE == "3")
                    {
                        isendflow = true;
                    }
                    project = projectService.GetEntity((until as IntromissionEntity).OUTENGINEERID);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    flowid = (until as IntromissionEntity).FLOWID;
                    table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' ", keyValue);
                    break;
                case "7":
                    until = new StartapplyforService().GetEntity(keyValue);
                    if ((until as StartapplyforEntity).IsOver == 1)
                    {
                        isendflow = true;
                    }
                    project = projectService.GetEntity((until as StartapplyforEntity).OUTENGINEERID);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    if (dataitemdetailbll.GetDataItemListByItemCode("FlowWithRiskLevel").Count() > 0)
                    {
                        moduleno = "KGSQSC" + project.ENGINEERLEVEL;
                    }
                    else
                    {
                        moduleno = "KGSQSC";
                    }
                        
                    flowid = (until as StartapplyforEntity).NodeId;
                    table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' ", keyValue);
                    break;
                case "8":
                    moduleno = "NBSGSJSH";
                    until = new PowerplantinsideService().GetEntity(keyValue);
                    if ((until as PowerplantinsideEntity).IsOver == 1)
                    {
                        isendflow = true;
                    }
                    //project = projectService.GetEntity((until as PowerplantinsideEntity));//��ȡ������Ϣ��ȡ��ִ�е�λ
                    flowid = (until as PowerplantinsideEntity).FlowID;
                    table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' and disable is null ", keyValue);
                    break;
                case "10":
                    moduleno = "ThreePeople";
                    until = new ThreePeopleCheckService().GetEntity(keyValue);
                    if ((until as ThreePeopleCheckEntity).IsOver == 1)
                    {
                        isendflow = true;
                    }
                    //project = projectService.GetEntity((until as PowerplantinsideEntity));//��ȡ������Ϣ��ȡ��ִ�е�λ
                    flowid = (until as ThreePeopleCheckEntity).NodeId;
                    table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}'", keyValue);
                    break;
                case "11":
                    moduleno = "AQDT";
                    until = new SecurityDynamicsService().GetEntity(keyValue);
                    if ((until as SecurityDynamicsEntity).ISOVER == "1")
                    {
                        isendflow = true;
                    }
                    //project = projectService.GetEntity((until as SecurityDynamicsEntity).);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    flowid = (until as SecurityDynamicsEntity).FlowId;
                    table = string.Format(@"left join
                                     (select b1.audittime,
 b1.auditresult,
 b1.auditdept,
 b1.auditpeople,
 b1.flowid
  from epg_aptitudeinvestigateaudit b1 where b1.aptitudeid = '{0}' and b1.Remark!='99') b on t.id = b.flowid ", keyValue);
                    break;
                case "12"://������ȫ����
                    moduleno = "JGAQYS";
                    until = new SafetyCollectService().GetEntity(keyValue);

                    //orgCode = (until as SafetyCollectEntity).CREATEUSERORGCODE;

                    //deptentityNew = new DepartmentService().GetEntityByCode((until as SafetyCollectEntity).CREATEUSERDEPTCODE);

                    if ((until as SafetyCollectEntity).ISOVER == "1")
                    {
                        isendflow = true;
                    }
                    //project = projectService.GetEntity((until as DangerChemicalsReceiveEntity).PROJECTID);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    flowid = (until as SafetyCollectEntity).FlowId;
                    //table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' ", keyValue);
                    table = string.Format(@"left join
                                     (select b1.audittime,
 b1.auditresult,
 b1.auditdept,
 b1.auditpeople,
 b1.flowid
  from epg_aptitudeinvestigateaudit b1 where b1.aptitudeid = '{0}' and b1.Remark!='99') b on t.id = b.flowid ", keyValue);
                    break;
                case "13": //��ȫ��������
                    until = new TechDisclosureService().GetEntity(keyValue);
                    if (string.IsNullOrWhiteSpace((until as TechDisclosureEntity).PROJECTID))
                    {
                        moduleno = "AQJSJD(SSGC)";
                        if ((until as TechDisclosureEntity).ENGINEERLEVEL == "001")
                        {
                            moduleno += "001";
                        }
                        else if ((until as TechDisclosureEntity).ENGINEERLEVEL == "002")
                        {
                            moduleno += "002";
                        }
                        else if ((until as TechDisclosureEntity).ENGINEERLEVEL == "003")
                        {
                            moduleno += "003";
                        }
                        
                    }
                    else
                    {
                        moduleno = "AQJSJD(XZGC)";
                        if ((until as TechDisclosureEntity).ENGINEERLEVEL == "001")
                        {
                            moduleno += "001";
                        }
                        else if ((until as TechDisclosureEntity).ENGINEERLEVEL == "002")
                        {
                            moduleno += "002";
                        }
                        else if ((until as TechDisclosureEntity).ENGINEERLEVEL == "003")
                        {
                            moduleno += "003";
                        }
                        project = projectService.GetEntity((until as TechDisclosureEntity).PROJECTID);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    }
                    flowid = (until as TechDisclosureEntity).FLOWID;
                    if ((until as TechDisclosureEntity).STATUS==3)
                    {
                        isendflow = true;
                    }
                    table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' ", keyValue);
                    break;
                case "14"://Σ��Ʒ����
                    moduleno = "WHPLY";
                    until = new DangerChemicalsReceiveService().GetEntity(keyValue);
                    orgCode = (until as DangerChemicalsReceiveEntity).CREATEUSERORGCODE;
                    deptentityNew = new DepartmentService().GetEntityByCode((until as DangerChemicalsReceiveEntity).CREATEUSERDEPTCODE);

                    if ((until as DangerChemicalsReceiveEntity).GrantState == 3)
                    {
                        isendflow = true;
                    }
                    //project = projectService.GetEntity((until as DangerChemicalsReceiveEntity).PROJECTID);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    flowid = (until as DangerChemicalsReceiveEntity).FlowId;
                    //table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' ", keyValue);
                    table = string.Format(@"left join
                                     (select b1.audittime,
 b1.auditresult,
 b1.auditdept,
 b1.auditpeople,
 b1.flowid
  from epg_aptitudeinvestigateaudit b1 where b1.aptitudeid = '{0}' and b1.Remark!='99') b on t.id = b.flowid ", keyValue);
                    break;
                default:
                    break;
            }
            string flowSql = string.Format(@"select t.flowname,t.id,t.serialnum,t.checkrolename,t.checkroleid,t.checkdeptid,t.checkdeptcode,
                                            b.auditresult,b.auditdept,b.auditpeople,b.audittime,t.checkdeptname,t.applytype,t.scriptcurcontent,t.choosedeptrange
                                             from  bis_manypowercheck t {2} 
                                             where t.createuserorgcode='{1}' and t.moduleno='{0}' order by t.serialnum asc", moduleno, currUser.OrganizeCode, table);
            DataTable dt = new DataTable();
            if (urltype=="10")
            {
                dt = BaseRepository().FindTable(string.Format("select t.flowname,t.id,t.serialnum,t.checkrolename,t.checkroleid,t.checkdeptid,t.checkdeptcode,t.checkdeptname,t.applytype,t.scriptcurcontent,t.choosedeptrange,'' auditresult,'' auditdept,'' auditpeople,'' audittime from bis_manypowercheck t where t.createuserorgcode='{0}' and t.moduleno='{1}' order by t.serialnum asc", currUser.OrganizeCode, moduleno));
                foreach(DataRow dr in dt.Rows)
                {
                    DataTable dtAudits = BaseRepository().FindTable(string.Format("select b.auditresult,b.auditdept,b.auditpeople,b.audittime from epg_aptitudeinvestigateaudit b where b.flowid='{0}' and aptitudeid='{1}' and disable is null order by createtime desc", dr[1].ToString(), keyValue));
                    if (dtAudits.Rows.Count>0)
                    {
                        dr["auditresult"] = dtAudits.Rows[0]["auditresult"].ToString();
                        dr["auditdept"] = dtAudits.Rows[0]["auditdept"].ToString();
                        dr["auditpeople"] = dtAudits.Rows[0]["auditpeople"].ToString();
                        dr["audittime"] = dtAudits.Rows[0]["audittime"].ToString();
                    }
                }
            }
            else
            {
                dt = this.BaseRepository().FindTable(flowSql);
            }
            #region urltype == "14"
            if (urltype == "14")
            {
                var clEntity = new DangerChemicalsService().GetEntity((until as DangerChemicalsReceiveEntity).MainId);
                if (clEntity != null)
                {
                    if (clEntity.IsScene == "�ֳ����")
                    {
                        dt = this.BaseRepository().FindTable(flowSql);
                        dt.Clear();
                        DataRow dr = dt.NewRow();
                        dr["flowname"] = "�������ύ����";
                        dr["id"] = Guid.NewGuid().ToString();
                        dr["serialnum"] = 1;
                        dr["checkrolename"] = (until as DangerChemicalsReceiveEntity).ReceiveUser;
                        dr["checkroleid"] = "";
                        dr["checkdeptid"] = (new UserService().GetEntity((until as DangerChemicalsReceiveEntity).ReceiveUserId)).DepartmentId;
                        dr["checkdeptcode"] = (new UserService().GetEntity((until as DangerChemicalsReceiveEntity).ReceiveUserId)).DepartmentCode;

                        if ((until as DangerChemicalsReceiveEntity).GrantState == 3)
                        {
                            dr["auditresult"] = "0";
                            dr["auditdept"] = new DepartmentService().GetEntity((new UserService().GetEntity((until as DangerChemicalsReceiveEntity).ReceiveUserId)).DepartmentId).FullName;
                            dr["auditpeople"] = (until as DangerChemicalsReceiveEntity).ReceiveUser;
                            if ((until as DangerChemicalsReceiveEntity).GrantDate.HasValue)
                            {
                                if ((until as DangerChemicalsReceiveEntity).GrantDate.Value != null)
                                {
                                    dr["audittime"] = (until as DangerChemicalsReceiveEntity).GrantDate;

                                }
                            }
                            flowid = dr["id"].ToString();
                        }
                        dr["checkdeptname"] = new DepartmentService().GetEntity((new UserService().GetEntity((until as DangerChemicalsReceiveEntity).ReceiveUserId)).DepartmentId).FullName;
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        dt = this.BaseRepository().FindTable(flowSql);
                        DataRow dr = dt.NewRow();
                        dr["flowname"] = "�����˷���";
                        dr["id"] = Guid.NewGuid().ToString();
                        dr["serialnum"] = dt.Rows.Count + 1;
                        dr["checkrolename"] = clEntity.GrantPerson;
                        dr["checkroleid"] = "";
                        dr["checkdeptid"] = (new UserService().GetEntity(clEntity.GrantPersonId)).DepartmentId;
                        dr["checkdeptcode"] = (new UserService().GetEntity(clEntity.GrantPersonId)).DepartmentCode;

                        if ((until as DangerChemicalsReceiveEntity).GrantState == 3)
                        {
                            dr["auditresult"] = "0";
                            dr["auditdept"] = new DepartmentService().GetEntity((new UserService().GetEntity((until as DangerChemicalsReceiveEntity).GrantUserId)).DepartmentId).FullName;
                            dr["auditpeople"] = clEntity.GrantPerson;
                            if ((until as DangerChemicalsReceiveEntity).GrantDate.HasValue)
                            {
                                if ((until as DangerChemicalsReceiveEntity).GrantDate.Value != null)
                                {
                                    dr["audittime"] = (until as DangerChemicalsReceiveEntity).GrantDate;

                                }
                            }
                            flowid = dr["id"].ToString();
                        }
                        if ((until as DangerChemicalsReceiveEntity).GrantState == 2)
                        {
                            flowid = dr["id"].ToString();
                        }
                        dr["checkdeptname"] = new DepartmentService().GetEntity((new UserService().GetEntity(clEntity.GrantPersonId)).DepartmentId).FullName;
                        dt.Rows.Add(dr);
                    }
                }
            }
            #endregion
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
                    nodes.name = dr["flowname"].ToString();
                    nodes.type = "stepnode";
                    //λ��
                    int m = i % 4;
                    int n = i / 4;
                    if (m == 0)
                    {
                        nodes.left = 120;
                    }
                    else
                    {
                        nodes.left = 120 + ((150 + 60) * m);
                    }
                    if (n == 0)
                    {
                        nodes.top = 54;
                    }
                    else
                    {
                        nodes.top = (n * 100) + 54;
                    }
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes.name;

                    var IsStartWorkInvestigate = false;
                    var IsEntranceInvestigate = false;
                    //�볧�����������Ƿ�����
                    string strSql = string.Format(@"select t.isuse,t.id,t.settingtype from EPG_INVESTIGATE t where t.orginezeid='{0}' and  t.settingtype='{1}'", currUser.OrganizeId, "�볧���");
                    DataTable dtRecord = this.BaseRepository().FindTable(strSql);
                    if (dtRecord.Rows.Count > 0) {
                        if (dtRecord.Rows[0]["isuse"].ToString() == "��")
                        {
                            if (urltype == "6" && Convert.ToInt32(dr["serialnum"]) == 1)
                            {
                                IsEntranceInvestigate = true;
                            }
                        }
                        else {
                            IsEntranceInvestigate = false;
                        }
                    } else {
                        IsEntranceInvestigate = false;
                    }
                    //����������������Ƿ�����
                    string strSql1 = string.Format(@"select t.isuse,t.id,t.settingtype from EPG_INVESTIGATE t where t.orginezeid='{0}' and  t.settingtype='{1}'", currUser.OrganizeId, "��������");
                    DataTable dtRecord1 = this.BaseRepository().FindTable(strSql1);
                    if (dtRecord1.Rows.Count > 0)
                    {
                        if (dtRecord1.Rows[0]["isuse"].ToString() == "��")
                        {
                            if (urltype == "7" && Convert.ToInt32(dr["serialnum"]) == 1)
                            {
                                IsStartWorkInvestigate = true;
                            }
                        }
                        else {
                            IsStartWorkInvestigate = false;
                        }
                    }
                    else
                    {
                        IsStartWorkInvestigate = false;
                    }

                    //�����������볡��ɵ�һ����Ҫ��ѯ����¼
                    if ((urltype == "6" && Convert.ToInt32(dr["serialnum"]) == 1 && IsEntranceInvestigate) || (urltype == "7" && Convert.ToInt32(dr["serialnum"]) == 1 && IsStartWorkInvestigate))
                    {
                        //��ѯ����¼
                        string sql = string.Format(@" select * from EPG_INVESTIGATERECORD t  where t.intofactoryid='{0}' and t.investigatetype='0'", keyValue);
                        DataTable dtItem = this.BaseRepository().FindTable(sql);
                        if (dtItem.Rows.Count > 0)
                        {
                            bool flag = true;
                            string person = string.Empty;
                            string deptName = string.Empty;
                            for (int j = 0; j < dtItem.Rows.Count; j++)
                            {
                                //��ѯ�����
                                string subSql = string.Format(@" select * from EPG_INVESTIGATEDTRECORD t where t.investigaterecordid='{0}' ", dtItem.Rows[j]["id"].ToString());
                                DataTable dtSubItem = this.BaseRepository().FindTable(subSql);
                                foreach (DataRow item in dtSubItem.Rows)
                                {
                                    if (item["investigateresult"].ToString() != "�����" && item["investigateresult"].ToString() != "�޴���")
                                    {
                                        flag = false;
                                        break;
                                    }
                                    else
                                    {
                                        if (string.IsNullOrWhiteSpace(person))
                                        {
                                            person += item["investigatepeople"].ToString() + ",";
                                            var pUser = new UserInfoService().GetUserInfoEntity(item["investigatepeopleid"].ToString());
                                            if (pUser != null) {
                                                if (string.IsNullOrWhiteSpace(deptName))
                                                {
                                                    deptName += pUser.DeptName + ",";
                                                }
                                                else
                                                {
                                                    if (!deptName.Contains(pUser.DeptName))
                                                    {
                                                        deptName += pUser.DeptName + ",";
                                                    }
                                                }
                                            }
                                           
                                        }
                                        else
                                        {
                                            if (!person.Contains(item["investigatepeople"].ToString()))
                                            {
                                                person += item["investigatepeople"].ToString() + ",";
                                                var pUser = new UserInfoService().GetUserInfoEntity(item["investigatepeopleid"].ToString());
                                                if (pUser != null) {
                                                    if (string.IsNullOrWhiteSpace(deptName))
                                                    {
                                                        deptName += pUser.DeptName + ",";
                                                    }
                                                    else
                                                    {
                                                        if (!deptName.Contains(pUser.DeptName))
                                                        {
                                                            deptName += pUser.DeptName + ",";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (!string.IsNullOrWhiteSpace(deptName))
                                {
                                    deptName = deptName.Substring(0, deptName.Length - 1);
                                }
                                if (!string.IsNullOrWhiteSpace(person))
                                {
                                    person = person.Substring(0, person.Length - 1);
                                }
                                if (flag)
                                {
                                    sinfo.Taged = 1;
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                    DateTime auditdate;
                                    DateTime.TryParse(dtItem.Rows[j]["createdate"].ToString(), out auditdate);
                                    nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                    nodedesignatedata.creatdept = deptName;
                                    nodedesignatedata.createuser = person;
                                    nodedesignatedata.status = "ͬ��";
                                    if (i == 0)
                                    {
                                        nodedesignatedata.prevnode = "��";
                                    }
                                    else
                                    {
                                        nodedesignatedata.prevnode = dt.Rows[i - 1]["flowname"].ToString();
                                    }
                                    nodelist.Add(nodedesignatedata);
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                                else
                                {
                                    sinfo.Taged = 0;
                                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                    NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                    nodedesignatedata.createdate = "��";
                                    if (dr["applytype"].ToString() == "0")
                                    {
                                        var checkDeptId = dr["checkdeptid"].ToString();
                                        if (checkDeptId == "-1")//������̱�� -1 Ϊִ�е�λ=������̵ķ�����λ
                                        {
                                            var deptentity = new DepartmentService().GetEntity(project.ENGINEERLETDEPTID);
                                            
                                            if (deptentity != null)
                                            {
                                                switch (dr["choosedeptrange"].ToString()) //�жϲ��ŷ�Χ
                                                {
                                                    case "0":
                                                        checkDeptId = deptentity.DepartmentId;
                                                        nodedesignatedata.creatdept = deptentity.FullName;
                                                        break;
                                                    case "1":
                                                        var dept = departmentservice.GetEntity(deptentity.DepartmentId);
                                                        while (dept.Nature != "����")
                                                        {
                                                            dept = departmentservice.GetEntity(dept.ParentId);
                                                        }
                                                        checkDeptId = dept.DepartmentId;
                                                        nodedesignatedata.creatdept = dept.FullName;
                                                        break;
                                                    case "2":
                                                        var dept1 = departmentservice.GetEntity(deptentity.DepartmentId);
                                                        while (dept1.Nature != "����")
                                                        {
                                                            dept1 = departmentservice.GetEntity(dept1.ParentId);
                                                        }
                                                        checkDeptId = (dept1.DepartmentId + "," + deptentity.DepartmentId).Trim(',');
                                                        nodedesignatedata.creatdept = (dept1.FullName + "," + deptentity.FullName).Trim(',');
                                                        break;
                                                    default:
                                                        checkDeptId = deptentity.DepartmentId;
                                                        nodedesignatedata.creatdept = deptentity.FullName;
                                                        break;
                                                }
                                                
                                            }
                                            else
                                            {
                                                nodedesignatedata.creatdept = "��";
                                            }
                                            if (urltype == "10")
                                            {
                                                ThreePeopleCheckEntity th = until as ThreePeopleCheckEntity;
                                                string deptId = "";
                                                if (th.ApplyType == "�ڲ�")
                                                {
                                                    deptId = th.BelongDeptId;
                                                }
                                                else
                                                {
                                                    string sql1 = string.Format("select ENGINEERLETDEPTID from EPG_OUTSOURINGENGINEER t where id='{0}'", th.ProjectId);
                                                    DataTable dtPeoject = BaseRepository().FindTable(sql1);
                                                    if (dtPeoject.Rows.Count > 0)
                                                    {
                                                        deptId = dtPeoject.Rows[0][0].ToString();
                                                    }
                                                }
                                                deptentity = new DepartmentService().GetEntity(deptId);
                                                if (deptentity != null)
                                                {
                                                    checkDeptId = deptentity.DepartmentId;
                                                    nodedesignatedata.creatdept = deptentity.FullName;
                                                }
                                                else
                                                {
                                                    nodedesignatedata.creatdept = "��";
                                                }
                                            }
                                        }
                                        else if (checkDeptId == "-2")
                                        {
                                            var deptentity = new DepartmentService().GetEntity(project.OUTPROJECTID);
                                            if (deptentity != null)
                                            {
                                                checkDeptId = deptentity.DepartmentId;
                                                nodedesignatedata.creatdept = deptentity.FullName;
                                            }
                                            else
                                            {
                                                nodedesignatedata.creatdept = "��";
                                            }
                                        }
                                        else if (checkDeptId == "-3" && urltype == "10")
                                        {
                                            var deptentity = new DepartmentService().GetEntity((until as ThreePeopleCheckEntity).CreateUserDeptId);
                                            if (deptentity != null)
                                            {
                                                checkDeptId = deptentity.DepartmentId;
                                                nodedesignatedata.creatdept = deptentity.FullName;
                                            }
                                            else
                                            {
                                                nodedesignatedata.creatdept = "��";
                                            }
                                        }
                                       
                                        else if (checkDeptId == "-6")
                                        {
                                            if (!string.IsNullOrEmpty(project.SupervisorId))
                                            {
                                                var deptentity = new DepartmentService().GetEntity(project.SupervisorId);
                                                if (deptentity != null)
                                                {
                                                    checkDeptId = deptentity.DepartmentId;
                                                    nodedesignatedata.creatdept = deptentity.FullName;
                                                }
                                                else
                                                {
                                                    nodedesignatedata.creatdept = "��";
                                                }
                                            }
                                            else {
                                                nodedesignatedata.creatdept = "��";
                                            }
                                            
                                        }
                                        else
                                        {
                                            nodedesignatedata.creatdept = dr["checkdeptname"].ToString();
                                        }
                                        string userNames = new ScaffoldService().GetUserName(checkDeptId, dr["checkrolename"].ToString(), "0").Split('|')[0];
                                        nodedesignatedata.createuser = !string.IsNullOrEmpty(userNames) ? userNames : "��";
                                    }
                                    else if (dr["applytype"].ToString() == "1")
                                    {
                                        var parameter = new List<DbParameter>();
                                        //ȡ�ű�����ȡ�˻��ķ�Χ��Ϣ
                                        if (dr["scriptcurcontent"].ToString().Contains("@outengineerid"))
                                        {
                                            parameter.Add(DbParameters.CreateDbParameter("@outengineerid", !string.IsNullOrEmpty(project.ID) ? project.ID : ""));
                                        }
                                        DbParameter[] arrayparam = parameter.ToArray();
                                        string userIds = DbFactory.Base().FindList<UserEntity>(dr["scriptcurcontent"].ToString(), arrayparam).Cast<UserEntity>().Aggregate("", (current, user) => current + (user.UserId + ",")).Trim(',');
                                        DataTable users = userservice.GetUserTable(userIds.Split(','));
                                        string[] usernames = users.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                                        string[] deptnames = users.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                                        nodedesignatedata.creatdept = string.Join(",", deptnames);
                                        nodedesignatedata.createuser = string.Join(",", usernames);
                                    }
                                    

                                    nodedesignatedata.status = "��";
                                    if (i == 0)
                                    {
                                        nodedesignatedata.prevnode = "��";
                                    }
                                    else
                                    {
                                        nodedesignatedata.prevnode = dt.Rows[i - 1]["flowname"].ToString();
                                    }

                                    nodelist.Add(nodedesignatedata);
                                    sinfo.NodeDesignateData = nodelist;
                                    nodes.setInfo = sinfo;
                                }
                            }
                        }
                        else
                        {
                            if (dr["id"].ToString() == flowid)
                            {
                                sinfo.Taged = 0;
                            }
                            List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            nodedesignatedata.createdate = "��";
                            if (dr["applytype"].ToString() == "0")
                            {
                                var checkDeptId = dr["checkdeptid"].ToString();
                                if (checkDeptId == "-1")//������̱�� -1 Ϊִ�е�λ=������̵ķ�����λ
                                {
                                    var deptentity = new DepartmentService().GetEntity(project.ENGINEERLETDEPTID);
                                    if (deptentity != null)
                                    {
                                        switch (dr["choosedeptrange"].ToString()) //�жϲ��ŷ�Χ
                                        {
                                            case "0":
                                                checkDeptId = deptentity.DepartmentId;
                                                nodedesignatedata.creatdept = deptentity.FullName;
                                                break;
                                            case "1":
                                                var dept = departmentservice.GetEntity(deptentity.DepartmentId);
                                                while (dept.Nature != "����")
                                                {
                                                    dept = departmentservice.GetEntity(dept.ParentId);
                                                }
                                                checkDeptId = dept.DepartmentId;
                                                nodedesignatedata.creatdept = dept.FullName;
                                                break;
                                            case "2":
                                                var dept1 = departmentservice.GetEntity(deptentity.DepartmentId);
                                                while (dept1.Nature != "����")
                                                {
                                                    dept1 = departmentservice.GetEntity(dept1.ParentId);
                                                }
                                                checkDeptId = (dept1.DepartmentId + "," + deptentity.DepartmentId).Trim(',');
                                                nodedesignatedata.creatdept = (dept1.FullName + "," + deptentity.FullName).Trim(',');
                                                break;
                                            default:
                                                checkDeptId = deptentity.DepartmentId;
                                                nodedesignatedata.creatdept = deptentity.FullName;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        nodedesignatedata.creatdept = "��";
                                    }
                                    if (urltype == "10")
                                    {
                                        ThreePeopleCheckEntity th = until as ThreePeopleCheckEntity;
                                        string deptId = "";
                                        if (th.ApplyType == "�ڲ�")
                                        {
                                            deptId = th.BelongDeptId;
                                        }
                                        else
                                        {
                                            string sql1 = string.Format("select ENGINEERLETDEPTID from EPG_OUTSOURINGENGINEER t where id='{0}'", th.ProjectId);
                                            DataTable dtPeoject = BaseRepository().FindTable(sql1);
                                            if (dtPeoject.Rows.Count > 0)
                                            {
                                                deptId = dtPeoject.Rows[0][0].ToString();
                                            }
                                        }
                                        deptentity = new DepartmentService().GetEntity(deptId);
                                        if (deptentity != null)
                                        {
                                            checkDeptId = deptentity.DepartmentId;
                                            nodedesignatedata.creatdept = deptentity.FullName;
                                        }
                                        else
                                        {
                                            nodedesignatedata.creatdept = "��";
                                        }
                                    }
                                }
                                else if (checkDeptId == "-2")//������̱�� -1 Ϊִ�е�λ=������̵ķ�����λ
                                {
                                    var deptentity = new DepartmentService().GetEntity(project.OUTPROJECTID);
                                    if (deptentity != null)
                                    {
                                        checkDeptId = deptentity.DepartmentId;
                                        nodedesignatedata.creatdept = deptentity.FullName;
                                    }
                                    else
                                    {
                                        nodedesignatedata.creatdept = "��";
                                    }
                                }
                                else if (checkDeptId == "-3" && urltype == "10")
                                {
                                    var deptentity = new DepartmentService().GetEntity((until as ThreePeopleCheckEntity).CreateUserDeptId);
                                    if (deptentity != null)
                                    {
                                        checkDeptId = deptentity.DepartmentId;
                                        nodedesignatedata.creatdept = deptentity.FullName;
                                    }
                                    else
                                    {
                                        nodedesignatedata.creatdept = "��";
                                    }
                                }
                            
                                else if (checkDeptId == "-6")
                                {
                                    if (!string.IsNullOrEmpty(project.SupervisorId))
                                    {
                                        var deptentity = new DepartmentService().GetEntity(project.SupervisorId);
                                        if (deptentity != null)
                                        {
                                            checkDeptId = deptentity.DepartmentId;
                                            nodedesignatedata.creatdept = deptentity.FullName;
                                        }
                                        else
                                        {
                                            nodedesignatedata.creatdept = "��";
                                        }
                                    }
                                    else
                                    {
                                        nodedesignatedata.creatdept = "��";
                                    }

                                }
                                else
                                {
                                    nodedesignatedata.creatdept = dr["checkdeptname"].ToString();
                                }


                                string userNames = new ScaffoldService().GetUserName(checkDeptId, dr["checkrolename"].ToString(), "0").Split('|')[0];
                                nodedesignatedata.createuser = !string.IsNullOrEmpty(userNames) ? userNames : "��";
                            }
                            else if (dr["applytype"].ToString() == "1")
                            {
                                var parameter = new List<DbParameter>();
                                //ȡ�ű�����ȡ�˻��ķ�Χ��Ϣ
                                if (dr["scriptcurcontent"].ToString().Contains("@outengineerid"))
                                {
                                    parameter.Add(DbParameters.CreateDbParameter("@outengineerid", !string.IsNullOrEmpty(project.ID) ? project.ID : ""));
                                }
                                DbParameter[] arrayparam = parameter.ToArray();
                                string userIds = DbFactory.Base().FindList<UserEntity>(dr["scriptcurcontent"].ToString(),arrayparam).Cast<UserEntity>().Aggregate("", (current, user) => current + (user.UserId + ",")).Trim(',');
                                DataTable users = userservice.GetUserTable(userIds.Split(','));
                                string[] usernames = users.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                                string[] deptnames = users.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                                nodedesignatedata.creatdept = string.Join(",", deptnames);
                                nodedesignatedata.createuser = string.Join(",", usernames);
                            }


                            nodedesignatedata.status = "��";
                            if (i == 0)
                            {
                                nodedesignatedata.prevnode = "��";
                            }
                            else
                            {
                                nodedesignatedata.prevnode = dt.Rows[i - 1]["flowname"].ToString();
                            }

                            nodelist.Add(nodedesignatedata);
                            sinfo.NodeDesignateData = nodelist;
                            nodes.setInfo = sinfo;
                        }
                        nlist.Add(nodes);
                    }
                    else
                    {
                        //��˼�¼
                        if (dr["auditdept"] != null && !string.IsNullOrWhiteSpace(dr["auditdept"].ToString()))
                        {
                            if (urltype == "10")
                            {
                                if ((until as ThreePeopleCheckEntity).IsSumbit == 1)
                                {
                                    sinfo.Taged = 1;
                                }
                                if ((until as ThreePeopleCheckEntity).NodeId ==dr["id"].ToString())
                                {
                                    sinfo.Taged =0;
                                }
                            }
                            else
                            {
                                sinfo.Taged = 1;
                            }
                            List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            DateTime auditdate;
                            if (sinfo.Taged == 1)
                            {
                                DateTime.TryParse(dr["audittime"].ToString(), out auditdate);
                                nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                nodedesignatedata.creatdept = dr["auditdept"].ToString();
                                nodedesignatedata.createuser = dr["auditpeople"].ToString();
                                nodedesignatedata.status = dr["auditresult"].ToString() == "0" ? "ͬ��" : "��ͬ��";
                            }
                            else
                            {
                                nodedesignatedata.createdate ="";
                                nodedesignatedata.creatdept ="";
                                nodedesignatedata.createuser = "";
                                nodedesignatedata.status = "";
                            }
                            if (i == 0)
                            {
                                nodedesignatedata.prevnode = "��";
                            }
                            else
                            {
                                nodedesignatedata.prevnode = dt.Rows[i - 1]["flowname"].ToString();
                            }

                            nodelist.Add(nodedesignatedata);
                            sinfo.NodeDesignateData = nodelist;
                            nodes.setInfo = sinfo;
                        }
                        else
                        {

                            if (dr["id"].ToString() == flowid)
                            {
                                sinfo.Taged = 0;
                            }
                            List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            nodedesignatedata.createdate = "��";
                            if (dr["applytype"].ToString() == "0")
                            {
                                var checkDeptId = dr["checkdeptid"].ToString();
                                if (checkDeptId == "-1")//������̱�� -1 Ϊִ�е�λ=������̵ķ�����λ
                                {
                                    var deptentity = new DepartmentService().GetEntity(project.ENGINEERLETDEPTID);
                                    if (deptentity != null)
                                    {
                                        switch (dr["choosedeptrange"].ToString()) //�жϲ��ŷ�Χ
                                        {
                                            case "0":
                                                checkDeptId = deptentity.DepartmentId;
                                                nodedesignatedata.creatdept = deptentity.FullName;
                                                break;
                                            case "1":
                                                var dept = departmentservice.GetEntity(deptentity.DepartmentId);
                                                while (dept.Nature != "����")
                                                {
                                                    dept = departmentservice.GetEntity(dept.ParentId);
                                                }
                                                checkDeptId = dept.DepartmentId;
                                                nodedesignatedata.creatdept = dept.FullName;
                                                break;
                                            case "2":
                                                var dept1 = departmentservice.GetEntity(deptentity.DepartmentId);
                                                while (dept1.Nature != "����")
                                                {
                                                    dept1 = departmentservice.GetEntity(dept1.ParentId);
                                                }
                                                checkDeptId = (dept1.DepartmentId + "," + deptentity.DepartmentId).Trim(',');
                                                nodedesignatedata.creatdept = (dept1.FullName + "," + deptentity.FullName).Trim(',');
                                                break;
                                            default:
                                                checkDeptId = deptentity.DepartmentId;
                                                nodedesignatedata.creatdept = deptentity.FullName;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        nodedesignatedata.creatdept = "��";
                                    }
                                    if (urltype == "10")
                                    {
                                        ThreePeopleCheckEntity th = until as ThreePeopleCheckEntity;
                                        string deptId = "";
                                        if (th.ApplyType == "�ڲ�")
                                        {
                                            deptId = th.BelongDeptId;
                                        }
                                        else
                                        {
                                            string sql1 = string.Format("select ENGINEERLETDEPTID from EPG_OUTSOURINGENGINEER t where id='{0}'", th.ProjectId);
                                            DataTable dtPeoject = BaseRepository().FindTable(sql1);
                                            if (dtPeoject.Rows.Count > 0)
                                            {
                                                deptId = dtPeoject.Rows[0][0].ToString();
                                            }
                                        }
                                        deptentity = new DepartmentService().GetEntity(deptId);
                                        if (deptentity != null)
                                        {
                                            checkDeptId = deptentity.DepartmentId;
                                            nodedesignatedata.creatdept = deptentity.FullName;
                                        }
                                        else
                                        {
                                            nodedesignatedata.creatdept = "��";
                                        }
                                    }
                                    if (urltype == "12")
                                    {
                                        SafetyCollectEntity sc = until as SafetyCollectEntity;
                                        var outsouringengineer = new OutsouringengineerService().GetEntity(sc.EngineerId);

                                        deptentity = new DepartmentService().GetEntity(outsouringengineer.ENGINEERLETDEPTID);
                                        if (deptentity != null)
                                        {
                                            //checkDeptId = deptentity.DepartmentId;
                                            //nodedesignatedata.creatdept = deptentity.FullName;
                                            var createdeptentity2 = new DepartmentEntity();
                                            while (deptentity.Nature == "רҵ" || deptentity.Nature == "����")
                                            {
                                                createdeptentity2 = new DepartmentService().GetEntity(deptentity.ParentId);
                                                if (createdeptentity2.Nature != "רҵ" || createdeptentity2.Nature != "����")
                                                {
                                                    break;
                                                }
                                            }
                                            checkDeptId = deptentity.DepartmentId;
                                            nodedesignatedata.creatdept = deptentity.FullName;
                                            if (createdeptentity2 != null)
                                            {
                                                checkDeptId = deptentity.DepartmentId + "," + createdeptentity2.DepartmentId;
                                                nodedesignatedata.creatdept = deptentity.FullName + "," + createdeptentity2.FullName;
                                            }
                                        }
                                        else
                                        {
                                            nodedesignatedata.creatdept = "��";
                                        }
                                    }
                                }
                                else if (checkDeptId == "-2")
                                {
                                    var deptentity = new DepartmentService().GetEntity(project.OUTPROJECTID);
                                    if (deptentity != null)
                                    {
                                        checkDeptId = deptentity.DepartmentId;
                                        nodedesignatedata.creatdept = deptentity.FullName;
                                    }
                                    else
                                    {
                                        nodedesignatedata.creatdept = "��";
                                    }
                                }
                                else if (checkDeptId == "-3" && urltype == "10")
                                {
                                    var deptentity = new DepartmentService().GetEntity((until as ThreePeopleCheckEntity).CreateUserDeptId);
                                    if (deptentity != null)
                                    {
                                        checkDeptId = deptentity.DepartmentId;
                                        nodedesignatedata.creatdept = deptentity.FullName;
                                    }
                                    else
                                    {
                                        nodedesignatedata.creatdept = "��";
                                    }
                                }
                                else if (checkDeptId == "-3" && urltype == "14")
                                {
                                    string deptId = (new DepartmentService().GetEntityByCode((until as DangerChemicalsReceiveEntity).CREATEUSERDEPTCODE)).DepartmentId;
                                    var deptentity = new DepartmentService().GetEntity(deptId);


                                    if (deptentity != null)
                                    {
                                        switch (dr["choosedeptrange"].ToString())
                                        {
                                            case "0":
                                                checkDeptId = deptentity.DepartmentId;
                                                nodedesignatedata.creatdept = deptentity.FullName;
                                                break;
                                            case "1":
                                                var dept = departmentservice.GetEntity(deptentity.DepartmentId);
                                                while (dept.Nature == "����" || dept.Nature == "רҵ")
                                                {
                                                    dept = departmentservice.GetEntity(dept.ParentId);
                                                }
                                                checkDeptId = dept.DepartmentId;
                                                nodedesignatedata.creatdept = dept.FullName;
                                                break;
                                            case "2":
                                                var dept1 = departmentservice.GetEntity(deptentity.DepartmentId);
                                                while (dept1.Nature == "����" || dept1.Nature == "רҵ")
                                                {
                                                    dept1 = departmentservice.GetEntity(dept1.ParentId);
                                                }
                                                checkDeptId = (dept1.DepartmentId + "," + deptentity.DepartmentId).Trim(',');
                                                nodedesignatedata.creatdept = (dept1.FullName + "," + deptentity.FullName).Trim(',');
                                                break;
                                            default:
                                                checkDeptId = deptentity.DepartmentId;
                                                nodedesignatedata.creatdept = deptentity.FullName;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        nodedesignatedata.creatdept = "��";
                                    }
                                }

                                else if (checkDeptId == "-6")
                                {
                                    if (!string.IsNullOrEmpty(project.SupervisorId))
                                    {
                                        var deptentity = new DepartmentService().GetEntity(project.SupervisorId);
                                        if (deptentity != null)
                                        {
                                            checkDeptId = deptentity.DepartmentId;
                                            nodedesignatedata.creatdept = deptentity.FullName;
                                        }
                                        else
                                        {
                                            nodedesignatedata.creatdept = "��";
                                        }
                                    }
                                    else
                                    {
                                        nodedesignatedata.creatdept = "��";
                                    }

                                }
                                else
                                {
                                    nodedesignatedata.creatdept = dr["checkdeptname"].ToString();
                                }
                                string userNames = new ScaffoldService().GetUserName(checkDeptId, dr["checkrolename"].ToString(), "0").Split('|')[0];
                                nodedesignatedata.createuser = !string.IsNullOrEmpty(userNames) ? userNames : "��";
                            }
                            else if (dr["applytype"].ToString() == "1")
                            {
                                var parameter = new List<DbParameter>();
                                //ȡ�ű�����ȡ�˻��ķ�Χ��Ϣ
                                if (dr["scriptcurcontent"].ToString().Contains("@outengineerid"))
                                {
                                    parameter.Add(DbParameters.CreateDbParameter("@outengineerid", !string.IsNullOrEmpty(project.ID) ? project.ID : ""));
                                }
                                if (dr["scriptcurcontent"].ToString().Contains("@engineerletdeptid")) //����ʲ���ݲ��Ų�ѯ�������ύ��ʱ��û��id���ű�ֻ�ܸ��ݲ������飩
                                {
                                    parameter.Add(DbParameters.CreateDbParameter("@engineerletdeptid", !string.IsNullOrEmpty(kbengineerletdeptid) ? kbengineerletdeptid : ""));
                                }
                                if (dr["scriptcurcontent"].ToString().Contains("@id"))
                                {
                                    parameter.Add(DbParameters.CreateDbParameter("@id", keyValue));
                                }
                                DbParameter[] arrayparam = parameter.ToArray();
                                string userIds = DbFactory.Base().FindList<UserEntity>(dr["scriptcurcontent"].ToString(), arrayparam).Cast<UserEntity>().Aggregate("", (current, user) => current + (user.UserId + ",")).Trim(',');
                                DataTable users = userservice.GetUserTable(userIds.Split(','));
                                string[] usernames = users.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                                string[] deptnames = users.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                                nodedesignatedata.creatdept = string.Join(",", deptnames);
                                nodedesignatedata.createuser = string.Join(",", usernames);
                            }
                            else
                            {
                                nodedesignatedata.creatdept = dr["checkdeptname"].ToString();
                                nodedesignatedata.createuser= dr["checkrolename"].ToString(); 
                            }

                            nodedesignatedata.status = "��";
                            if (i == 0)
                            {
                                nodedesignatedata.prevnode = "��";
                            }
                            else
                            {
                                nodedesignatedata.prevnode = dt.Rows[i - 1]["flowname"].ToString();
                            }

                            nodelist.Add(nodedesignatedata);
                            sinfo.NodeDesignateData = nodelist;
                            nodes.setInfo = sinfo;
                        }
                        nlist.Add(nodes);
                    }
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
                nodes_end.left = nlist[nlist.Count - 1].left;
                nodes_end.top = nlist[nlist.Count - 1].top + 100;
                nlist.Add(nodes_end);

                //���״̬Ϊ���ͨ����ͨ�������̽������б�ʶ 
                if (isendflow)
                {
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes_end.name;
                    sinfo.Taged = 1;
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    NodeDesignateData nodedesignatedata = new NodeDesignateData();

                    //ȡ���̽���ʱ�Ľڵ���Ϣ
                    if (!string.IsNullOrWhiteSpace(flowid))
                    {
                        DataRow[] end_rows = dt.Select("id = '" + flowid + "'");
                        if (end_rows.Length > 0)
                        {
                            DataRow end_row = end_rows[0];
                            DateTime auditdate;
                            DateTime.TryParse(end_row["audittime"].ToString(), out auditdate);
                            nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                            nodedesignatedata.creatdept = end_row["auditdept"].ToString();
                            nodedesignatedata.createuser = end_row["auditpeople"].ToString();
                            nodedesignatedata.status = end_row["auditresult"].ToString() == "0" ? "ͬ��" : "��ͬ��";
                            nodedesignatedata.prevnode = end_row["flowname"].ToString();
                        }
                        else
                        {
                            if (dt.Rows.Count > 0)
                            {
                                int len = dt.Rows.Count;
                                DataRow end_row = dt.Rows[len - 1];
                                DateTime auditdate;
                                DateTime.TryParse(end_row["audittime"].ToString(), out auditdate);
                                nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                nodedesignatedata.creatdept = end_row["auditdept"].ToString();
                                nodedesignatedata.createuser = end_row["auditpeople"].ToString();
                                nodedesignatedata.status = end_row["auditresult"].ToString() == "0" ? "ͬ��" : "��ͬ��";
                                nodedesignatedata.prevnode = end_row["flowname"].ToString();
                            }
                            else
                            {
                                int len = dt.Rows.Count;
                                nodedesignatedata.createdate = "";
                                nodedesignatedata.creatdept = "";
                                nodedesignatedata.createuser = "";
                                nodedesignatedata.status = "";
                                nodedesignatedata.prevnode = "";
                            }

                        }
                    }
                    else
                    {
                        if (dt.Rows.Count > 0)
                        {
                            int len = dt.Rows.Count;
                            DataRow end_row = dt.Rows[len - 1];
                            DateTime auditdate;
                            DateTime.TryParse(end_row["audittime"].ToString(), out auditdate);
                            nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                            nodedesignatedata.creatdept = end_row["auditdept"].ToString();
                            nodedesignatedata.createuser = end_row["auditpeople"].ToString();
                            nodedesignatedata.status = end_row["auditresult"].ToString() == "0" ? "ͬ��" : "��ͬ��";
                            nodedesignatedata.prevnode = end_row["flowname"].ToString();
                        }
                        else
                        {
                            int len = dt.Rows.Count;
                            nodedesignatedata.createdate = "";
                            nodedesignatedata.creatdept = "";
                            nodedesignatedata.createuser = "";
                            nodedesignatedata.status = "";
                            nodedesignatedata.prevnode = "";
                        }
                    }
                    nodelist.Add(nodedesignatedata);
                    sinfo.NodeDesignateData = nodelist;
                    nodes_end.setInfo = sinfo;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lines lines = new lines();
                    lines.alt = true;
                    lines.id = Guid.NewGuid().ToString();
                    lines.from = dt.Rows[i]["id"].ToString();
                    if (i < dt.Rows.Count - 1)
                    {
                        lines.to = dt.Rows[i + 1]["id"].ToString();
                    }
                    lines.name = "";
                    lines.type = "sl";
                    llist.Add(lines);
                }

                lines lines_end = new lines();
                lines_end.alt = true;
                lines_end.id = Guid.NewGuid().ToString();
                lines_end.from = dt.Rows[dt.Rows.Count - 1]["id"].ToString();
                lines_end.to = nodes_end.id;
                llist.Add(lines_end);

                flow.nodes = nlist;
                flow.lines = llist;
            }
            return flow;


        }


        /// <summary>
        /// ��ѯ�������ͼ-�ֻ���ʹ��
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="urltype">��ѯ���ͣ�1 ��λ���� 2 ��Ա���� 3 �����豸���� 4 �綯/��ȫ���������� 5�������� 6�볧��� 7�������� 8�ճ�����</param>
        /// <returns></returns>
        public List<CheckFlowList> GetAppCheckFlowList(string keyValue, string urltype, Operator currUser)
        {

            List<CheckFlowList> nodelist = new List<CheckFlowList>();
            var isendflow = false;//���̽������
            string flowid = string.Empty;
            string moduleno = string.Empty;//ģ���ʾ
            var until = new BaseEntity();//����ʵ��
            string table = string.Empty;
            var deptentityNew = new DepartmentEntity();
            switch (urltype)
            {
                case "1":
                    moduleno = "WHPLY";
                    until = new DangerChemicalsReceiveService().GetEntity(keyValue);
                    deptentityNew = new DepartmentService().GetEntityByCode((until as DangerChemicalsReceiveEntity).CREATEUSERDEPTCODE);
                    if ((until as DangerChemicalsReceiveEntity).GrantState == 3)
                    {
                        isendflow = true;
                    }
                    flowid = (until as DangerChemicalsReceiveEntity).FlowId;
                    table = string.Format(@"left join
                                     (select b1.audittime,
 b1.auditresult,
 b1.auditdept,
 b1.auditpeople,
 b1.auditopinion,
 b1.flowid,
 b1.AUDITSIGNIMG
  from epg_aptitudeinvestigateaudit b1 where b1.aptitudeid = '{0}' and b1.Remark!='99') b on t.id = b.flowid ", keyValue);
                    break;
 //               case "2":
 //                   moduleno = "WHPBF";
 //                   until = new DangerChemicalsScrapService().GetEntity(keyValue);
 //                   deptentityNew = new DepartmentService().GetEntityByCode((until as DangerChemicalsScrapEntity).CREATEUSERDEPTCODE);
 //                   if ((until as DangerChemicalsScrapEntity).ISOVER == "1")
 //                   {
 //                       isendflow = true;
 //                   }
 //                   flowid = (until as DangerChemicalsScrapEntity).FlowId;
 //                   table = string.Format(@"left join
 //                                    (select b1.audittime,
 //b1.auditresult,
 //b1.auditdept,
 //b1.auditpeople,
 //b1.auditopinion,
 //b1.flowid,
 //b1.AUDITSIGNIMG
 // from epg_aptitudeinvestigateaudit b1 where b1.aptitudeid = '{0}' and b1.Remark!='99') b on t.id = b.flowid ", keyValue);
 //                   break;
                default:
                    break;
            }
            string flowSql = string.Format(@"select t.flowname,t.id,t.serialnum,t.checkrolename,t.checkroleid,t.checkdeptid,t.checkdeptcode,
                                            b.auditresult,b.auditdept,b.auditpeople,b.audittime,t.checkdeptname,b.auditopinion,b.AUDITSIGNIMG
                                             from  bis_manypowercheck t {2} 
                                             where t.createuserorgcode='{1}' and t.moduleno='{0}' order by t.serialnum asc", moduleno, currUser.OrganizeCode, table);
            DataTable dt = this.BaseRepository().FindTable(flowSql);
            if (urltype == "1")
            {
                var clEntity = new DangerChemicalsService().GetEntity((until as DangerChemicalsReceiveEntity).MainId);
                if (clEntity != null)
                {
                    if (clEntity.IsScene == "�ֳ����")
                    {
                        dt = this.BaseRepository().FindTable(flowSql);
                        dt.Clear();
                        DataRow dr = dt.NewRow();
                        dr["flowname"] = "�������ύ����";
                        dr["id"] = Guid.NewGuid().ToString();
                        dr["serialnum"] = 1;
                        dr["checkrolename"] = (until as DangerChemicalsReceiveEntity).ReceiveUser;
                        dr["checkroleid"] = "";
                        dr["checkdeptid"] = (new UserService().GetEntity((until as DangerChemicalsReceiveEntity).ReceiveUserId)).DepartmentId;
                        dr["checkdeptcode"] = (new UserService().GetEntity((until as DangerChemicalsReceiveEntity).ReceiveUserId)).DepartmentCode;

                        if ((until as DangerChemicalsReceiveEntity).GrantState == 3)
                        {
                            dr["auditresult"] = "0";
                            dr["auditdept"] = new DepartmentService().GetEntity((new UserService().GetEntity((until as DangerChemicalsReceiveEntity).ReceiveUserId)).DepartmentId).FullName;
                            dr["auditpeople"] = (until as DangerChemicalsReceiveEntity).ReceiveUser;
                            if ((until as DangerChemicalsReceiveEntity).GrantDate.HasValue)
                            {
                                if ((until as DangerChemicalsReceiveEntity).GrantDate.Value != null)
                                {
                                    dr["audittime"] = (until as DangerChemicalsReceiveEntity).GrantDate;

                                }
                            }
                            flowid = dr["id"].ToString();
                        }
                        dr["checkdeptname"] = new DepartmentService().GetEntity((new UserService().GetEntity((until as DangerChemicalsReceiveEntity).ReceiveUserId)).DepartmentId).FullName;
                        dr["auditsignimg"] = (until as DangerChemicalsReceiveEntity).SignImg;
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        dt = this.BaseRepository().FindTable(flowSql);
                        DataRow dr = dt.NewRow();
                        dr["flowname"] = "�����˷���";
                        dr["id"] = Guid.NewGuid().ToString();
                        dr["serialnum"] = dt.Rows.Count + 1;
                        dr["checkrolename"] = clEntity.GrantPerson;
                        dr["checkroleid"] = "";
                        dr["checkdeptid"] = (new UserService().GetEntity(clEntity.GrantPersonId)).DepartmentId;
                        dr["checkdeptcode"] = (new UserService().GetEntity(clEntity.GrantPersonId)).DepartmentCode;

                        if ((until as DangerChemicalsReceiveEntity).GrantState == 3)
                        {
                            dr["auditresult"] = "0";
                            dr["auditdept"] = new DepartmentService().GetEntity((new UserService().GetEntity((until as DangerChemicalsReceiveEntity).GrantUserId)).DepartmentId).FullName;
                            dr["auditpeople"] = clEntity.GrantPerson;
                            if ((until as DangerChemicalsReceiveEntity).GrantDate.HasValue)
                            {
                                if ((until as DangerChemicalsReceiveEntity).GrantDate.Value != null)
                                {
                                    dr["audittime"] = (until as DangerChemicalsReceiveEntity).GrantDate;

                                }
                            }
                            flowid = dr["id"].ToString();
                        }
                        if ((until as DangerChemicalsReceiveEntity).GrantState == 2)
                        {
                            flowid = dr["id"].ToString();
                        }
                        dr["checkdeptname"] = new DepartmentService().GetEntity((new UserService().GetEntity(clEntity.GrantPersonId)).DepartmentId).FullName;
                        dr["auditsignimg"] = (until as DangerChemicalsReceiveEntity).GrantSignImg;
                        dr["auditopinion"] = (until as DangerChemicalsReceiveEntity).GrantOpinion;
                        dt.Rows.Add(dr);
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    //��˼�¼
                    if (dr["auditdept"] != null && !string.IsNullOrEmpty(dr["auditdept"].ToString()))
                    {
                        CheckFlowList checkdata = new CheckFlowList();
                        DateTime auditdate;
                        DateTime.TryParse(dr["audittime"].ToString(), out auditdate);
                        checkdata.auditdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        checkdata.auditdeptname = dr["auditdept"].ToString();
                        checkdata.auditusername = dr["auditpeople"].ToString();
                        checkdata.auditstate = dr["auditresult"].ToString() == "0" ? "ͬ��" : "��ͬ��";
                        checkdata.isapprove = "1";
                        checkdata.isoperate = "0";
                        checkdata.auditremark = dr["auditopinion"].ToString();
                        checkdata.signpic = dr["auditsignimg"].ToString();
                        nodelist.Add(checkdata);
                    }
                    else
                    {
                        CheckFlowList checkdata = new CheckFlowList();
                        checkdata.auditdate = "";
                        //����,��Ա
                        var checkDeptId = dr["checkdeptid"].ToString();
                        if (checkDeptId == "-1")
                        {
                            var deptentity = deptentityNew;
                            while (deptentity.Nature == "רҵ" || deptentity.Nature == "����")
                            {
                                deptentity = new DepartmentService().GetEntity(deptentity.ParentId);
                            }
                            if (deptentity != null)
                            {
                                checkDeptId = deptentity.DepartmentId;
                                checkdata.auditdeptname = deptentity.FullName;
                            }
                            else
                            {
                                checkdata.auditdeptname = "��";
                            }
                        }
                        else if (checkDeptId == "-2")
                        {
                            var deptentity = deptentityNew;
                            while (deptentity.Nature == "רҵ" || deptentity.Nature == "����")
                            {
                                deptentity = new DepartmentService().GetEntity(deptentity.ParentId);
                            }
                            if (deptentity != null)
                            {
                                checkDeptId = deptentity.DepartmentId;
                                checkdata.auditdeptname = deptentity.FullName;
                            }
                            else
                            {
                                checkdata.auditdeptname = "��";
                            }
                        }
                        else if (checkDeptId == "-3")
                        {
                            var deptentity =  new DepartmentService().GetEntityByCode((until as DangerChemicalsReceiveEntity).CREATEUSERDEPTCODE);
                            while (deptentity.Nature == "רҵ" || deptentity.Nature == "����")
                            {
                                deptentity = new DepartmentService().GetEntity(deptentity.ParentId);
                            }
                            if (deptentity != null)
                            {
                                checkDeptId = deptentity.DepartmentId;
                                checkdata.auditdeptname = deptentity.FullName;
                            }
                            else
                            {
                                checkdata.auditdeptname = "��";
                            }
                        }
                        else
                        {
                            checkdata.auditdeptname = dr["checkdeptname"].ToString();
                        }
                        string userNames = new ScaffoldService().GetUserName(checkDeptId, dr["checkrolename"].ToString(), "0").Split('|')[0];
                        checkdata.auditusername = !string.IsNullOrEmpty(userNames) ? userNames : "";

                        checkdata.auditremark = "";
                        checkdata.isapprove = "0";
                        if (isendflow)
                            checkdata.isoperate = "0";
                        else
                            checkdata.isoperate = dr["id"].ToString() == flowid ? "1" : "0";
                        if (checkdata.isoperate == "1")
                        {
                            checkdata.auditstate = "�����";
                            if (dr["flowname"].ToString() == "�����˷���")
                            {
                                checkdata.auditstate = "������";
                            }
                        }
                        nodelist.Add(checkdata);
                    }

                }
            }
            return nodelist;
        }

        /// <summary>
        /// ��ѯ�������ͼ-�ֻ���ʹ��
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="urltype">��ѯ���ͣ�1 ��λ���� 2 ��Ա���� 3 �����豸���� 4 �綯/��ȫ���������� 5�������� 6�볧��� 7�������� 8�ճ�����</param>
        /// <returns></returns>
        public List<CheckFlowData> GetAppFlowList(string keyValue, string urltype,Operator currUser)
        {
            
            List<CheckFlowData> nodelist = new List<CheckFlowData>();
            var isendflow = false;//���̽������
            string flowid = string.Empty;
            string moduleno = string.Empty;//ģ���ʾ
            var until = new BaseEntity();//����ʵ��
            OutsouringengineerEntity project = new OutsouringengineerEntity();//����ʵ��
            var projectService = new OutsouringengineerService();
            string table = string.Empty;
            string kbengineerletdeptid = string.Empty; // ����ʲ���̲���id
            switch (urltype)
            {
                case "1":
                    moduleno = "DWZZ";
                    until = this.BaseRepository().FindEntity(keyValue);
                    if ((until as AptitudeinvestigateinfoEntity).ISAUDITOVER == "1")
                    {
                        isendflow = true;
                    }
                    project = projectService.GetEntity((until as AptitudeinvestigateinfoEntity).OUTENGINEERID);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    flowid = (until as AptitudeinvestigateinfoEntity).FlowId;
                    table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' ", keyValue);
                    break;
                case "2":
                    moduleno = "RYZZ";
                    until = new PeopleReviewService().GetEntity(keyValue);
                    if ((until as PeopleReviewEntity).ISAUDITOVER == "1")
                    {
                        isendflow = true;
                    }
                    project = projectService.GetEntity((until as PeopleReviewEntity).OUTENGINEERID);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    flowid = (until as PeopleReviewEntity).FlowId;
                    table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' ", keyValue);
                    break;
                case "3":
                    moduleno = "TZSBGQJ";
                    until = new ToolsService().GetEntity(keyValue);
                    if ((until as ToolsEntity).ISOVER == "1")
                    {
                        isendflow = true;
                    }
                    project = projectService.GetEntity((until as ToolsEntity).OUTENGINEERID);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    //workdeptid = (until as ToolsEntity).FLOWDEPT;
                    flowid = (until as ToolsEntity).FlowId;
                    table = string.Format(@"left join epg_toolsaudit b on t.id = b.flowid and b.toolsid = '{0}' ", keyValue);
                    break;
                case "4":
                    moduleno = "SBGQJ";
                    until = new ToolsService().GetEntity(keyValue);
                    if ((until as ToolsEntity).ISOVER == "1")
                    {
                        isendflow = true;
                    }
                    project = projectService.GetEntity((until as ToolsEntity).OUTENGINEERID);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    flowid = (until as ToolsEntity).FlowId;
                    table = string.Format(@"left join epg_toolsaudit b on t.id = b.flowid and b.toolsid = '{0}' ", keyValue);
                    break;
                case "5":
                    until = new SchemeMeasureService().GetEntity(keyValue);
                    if ((until as SchemeMeasureEntity).ISOVER == "1")
                    {
                        isendflow = true;
                    }
                    project = projectService.GetEntity((until as SchemeMeasureEntity).PROJECTID);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    if (project == null) // �����������ʱ��
                    {
                        moduleno = "SCLA_SSGC";
                        kbengineerletdeptid = (until as SchemeMeasureEntity).ENGINEERLETDEPTID;
                    }
                    else
                    {
                        if (dataitemdetailbll.GetDataItemListByItemCode("FlowWithRiskLevel").Count() > 0)
                        {
                            moduleno = "SCLA" + project.ENGINEERLEVEL;
                        }
                        else
                        {
                            moduleno = "SCLA";
                        }
                    }
                    flowid = (until as SchemeMeasureEntity).FlowId;
                    table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' ", keyValue);
                    break;
                case "6":
                    moduleno = "RCXKSC";
                    until = new IntromissionService().GetEntity(keyValue);
                    if ((until as IntromissionEntity).INVESTIGATESTATE == "3")
                    {
                        isendflow = true;
                    }
                    project = projectService.GetEntity((until as IntromissionEntity).OUTENGINEERID);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    flowid = (until as IntromissionEntity).FLOWID;
                    table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' ", keyValue);
                    break;
                case "7":
                    until = new StartapplyforService().GetEntity(keyValue);
                    if ((until as StartapplyforEntity).IsOver == 1)
                    {
                        isendflow = true;
                    }
                    project = projectService.GetEntity((until as StartapplyforEntity).OUTENGINEERID);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    if (dataitemdetailbll.GetDataItemListByItemCode("FlowWithRiskLevel").Count() > 0)
                    {
                        moduleno = "KGSQSC" + project.ENGINEERLEVEL;
                    }
                    else
                    {
                        moduleno = "KGSQSC";
                    }
                        
                    flowid = (until as StartapplyforEntity).NodeId;
                    table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' ", keyValue);
                    break;
                case "8":
                    moduleno = "RCKH";
                    until = new DailyexamineService().GetEntity(keyValue);
                    if ((until as DailyexamineEntity).IsOver == 1)
                    {
                        isendflow = true;
                    }
                    flowid = (until as DailyexamineEntity).FlowID;
                    table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' ", keyValue);
                    break;
                case "13": //��ȫ��������
                    until = new TechDisclosureService().GetEntity(keyValue);
                    if (string.IsNullOrWhiteSpace((until as TechDisclosureEntity).PROJECTID))
                    {
                        moduleno = "AQJSJD(SSGC)";
                        if ((until as TechDisclosureEntity).ENGINEERLEVEL == "001")
                        {
                            moduleno += "001";
                        }
                        else if ((until as TechDisclosureEntity).ENGINEERLEVEL == "002")
                        {
                            moduleno += "002";
                        }
                        else if ((until as TechDisclosureEntity).ENGINEERLEVEL == "003")
                        {
                            moduleno += "003";
                        }

                    }
                    else
                    {
                        moduleno = "AQJSJD(XZGC)";
                        if ((until as TechDisclosureEntity).ENGINEERLEVEL == "001")
                        {
                            moduleno += "001";
                        }
                        else if ((until as TechDisclosureEntity).ENGINEERLEVEL == "002")
                        {
                            moduleno += "002";
                        }
                        else if ((until as TechDisclosureEntity).ENGINEERLEVEL == "003")
                        {
                            moduleno += "003";
                        }
                        project = projectService.GetEntity((until as TechDisclosureEntity).PROJECTID);//��ȡ������Ϣ��ȡ��ִ�е�λ
                    }
                    flowid = (until as TechDisclosureEntity).FLOWID;
                    if ((until as TechDisclosureEntity).STATUS == 3)
                    {
                        isendflow = true;
                    }
                    table = string.Format(@"left join epg_aptitudeinvestigateaudit b on t.id = b.flowid and b.aptitudeid = '{0}' ", keyValue);
                    break;
                default:
                    break;
            }
            string flowSql = string.Format(@"select t.flowname,t.id,t.serialnum,t.checkrolename,t.checkroleid,t.checkdeptid,t.checkdeptcode,t.applytype,t.scriptCurcontent,
                                            b.auditresult,b.auditdept,b.auditpeople,b.audittime,t.checkdeptname,b.auditopinion
                                             from  bis_manypowercheck t {2} 
                                             where t.createuserorgcode='{1}' and t.moduleno='{0}' order by t.serialnum asc", moduleno, currUser.OrganizeCode, table);
            DataTable dt = this.BaseRepository().FindTable(flowSql);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];

                    var IsStartWorkInvestigate = false;
                    var IsEntranceInvestigate = false;
                    //�볧�����������Ƿ�����
                    string strSql = string.Format(@"select t.isuse,t.id,t.settingtype from EPG_INVESTIGATE t where t.orginezeid='{0}' and  t.settingtype='{1}'", currUser.OrganizeId, "�볧���");
                    DataTable dtRecord = this.BaseRepository().FindTable(strSql);
                    if (dtRecord.Rows.Count > 0)
                    {
                        if (dtRecord.Rows[0]["isuse"].ToString() == "��")
                        {
                            if (urltype == "6" && Convert.ToInt32(dr["serialnum"]) == 1)
                            {
                                IsEntranceInvestigate = true;
                            }
                        }
                        else
                        {
                            IsEntranceInvestigate = false;
                        }
                    }
                    else
                    {
                        IsEntranceInvestigate = false;
                    }
                    //����������������Ƿ�����
                    string strSql1 = string.Format(@"select t.isuse,t.id,t.settingtype from EPG_INVESTIGATE t where t.orginezeid='{0}' and  t.settingtype='{1}'", currUser.OrganizeId, "��������");
                    DataTable dtRecord1 = this.BaseRepository().FindTable(strSql1);

                    if (dtRecord1.Rows.Count > 0)
                    {
                        if (dtRecord1.Rows[0]["isuse"].ToString() == "��")
                        {
                            if (urltype == "7" && Convert.ToInt32(dr["serialnum"]) == 1)
                            {
                                IsStartWorkInvestigate = true;
                            }
                        }
                        else
                        {
                            IsStartWorkInvestigate = false;
                        }
                    }
                    else
                    {
                        IsStartWorkInvestigate = false;
                    }
                    if ((urltype == "6" && Convert.ToInt32(dr["serialnum"]) == 1 && IsEntranceInvestigate) || (urltype == "7" && Convert.ToInt32(dr["serialnum"]) == 1 && IsStartWorkInvestigate))
                    {
                       
                        //��ѯ����¼
                        string sql = string.Format(@" select * from epg_investigaterecord t  where t.intofactoryid='{0}' and t.investigatetype='0'", keyValue);
                        DataTable dtItem = this.BaseRepository().FindTable(sql);
                        if (dtItem.Rows.Count > 0)
                        {
                            bool flag = true;
                            string person = string.Empty;
                            string deptName = string.Empty;
                            for (int j = 0; j < dtItem.Rows.Count; j++)
                            {
                                //��ѯ�����
                                string subSql = string.Format(@" select * from epg_investigatedtrecord t where t.investigaterecordid='{0}' ", dtItem.Rows[j]["id"].ToString());
                                DataTable dtSubItem = this.BaseRepository().FindTable(subSql);
                                foreach (DataRow item in dtSubItem.Rows)
                                {
                                    if (item["investigateresult"].ToString() != "�����" && item["investigateresult"].ToString() != "�޴���")
                                    {
                                        flag = false;
                                        break;
                                    }
                                    else
                                    {
                                        if (string.IsNullOrWhiteSpace(person))
                                        {
                                            person += item["investigatepeople"].ToString() + ",";
                                            var pUser = new UserInfoService().GetUserInfoEntity(item["investigatepeopleid"].ToString());
                                            if (pUser != null) {
                                                if (string.IsNullOrWhiteSpace(deptName))
                                                {
                                                    deptName += pUser.DeptName + ",";
                                                }
                                                else
                                                {
                                                    if (!deptName.Contains(pUser.DeptName))
                                                    {
                                                        deptName += pUser.DeptName + ",";
                                                    }
                                                }
                                            }
                                           
                                        }
                                        else
                                        {
                                            if (!person.Contains(item["investigatepeople"].ToString()))
                                            {
                                                person += item["investigatepeople"].ToString() + ",";
                                                var pUser = new UserInfoService().GetUserInfoEntity(item["investigatepeopleid"].ToString());
                                                if (pUser != null) {
                                                    if (string.IsNullOrWhiteSpace(deptName))
                                                    {
                                                        deptName += pUser.DeptName + ",";
                                                    }
                                                    else
                                                    {
                                                        if (!deptName.Contains(pUser.DeptName))
                                                        {
                                                            deptName += pUser.DeptName + ",";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (!string.IsNullOrWhiteSpace(deptName))
                                {
                                    deptName = deptName.Substring(0, deptName.Length - 1);
                                }
                                if (!string.IsNullOrWhiteSpace(person))
                                {
                                    person = person.Substring(0, person.Length - 1);
                                }
                                if (flag)
                                {
                                    CheckFlowData checkdata = new CheckFlowData();
                                    DateTime auditdate;
                                    DateTime.TryParse(dtItem.Rows[j]["createdate"].ToString(), out auditdate);
                                    checkdata.auditdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                    checkdata.auditdeptname = deptName;
                                    checkdata.auditusername = person;
                                    checkdata.auditstate = "ͬ��";
                                    checkdata.isapprove = "1";
                                    checkdata.isoperate = "0";
                                    nodelist.Add(checkdata);
                                }
                                else
                                {
                                    CheckFlowData checkdata = new CheckFlowData();
                                    checkdata.auditdate = "";
                                    //����,��Ա
                                    var checkDeptId = dr["checkdeptid"].ToString();
                                    if (dr["applytype"].ToString() == "0")
                                    {
                                        if (checkDeptId == "-1")
                                        {
                                            var deptentity = new DepartmentService().GetEntity(project.ENGINEERLETDEPTID);
                                            if (deptentity != null)
                                            {
                                                checkDeptId = deptentity.DepartmentId;
                                                checkdata.auditdeptname = deptentity.FullName;
                                            }
                                            else
                                            {
                                                checkdata.auditdeptname = "��";
                                            }
                                        }
                                        else if (checkDeptId == "-2")
                                        {
                                            var deptentity = new DepartmentService().GetEntity(project.OUTPROJECTID);
                                            if (deptentity != null)
                                            {
                                                checkDeptId = deptentity.DepartmentId;
                                                checkdata.auditdeptname = deptentity.FullName;
                                            }
                                            else
                                            {
                                                checkdata.auditdeptname = "��";
                                            }
                                        }
                                        else if (checkDeptId == "-6")
                                        {
                                            if (!string.IsNullOrEmpty(project.SupervisorId))
                                            {
                                                var deptentity = new DepartmentService().GetEntity(project.SupervisorId);
                                                if (deptentity != null)
                                                {
                                                    checkDeptId = deptentity.DepartmentId;
                                                    checkdata.auditdeptname = deptentity.FullName;
                                                }
                                                else
                                                {
                                                    checkdata.auditdeptname = "��";
                                                }
                                            }
                                            else
                                            {
                                                checkdata.auditdeptname = "��";
                                            }

                                        }
                                        else
                                        {
                                            checkdata.auditdeptname = dr["checkdeptname"].ToString();
                                        }
                                        string userNames = new ScaffoldService().GetUserName(checkDeptId, dr["checkrolename"].ToString(), "0").Split('|')[0];
                                        checkdata.auditusername = !string.IsNullOrEmpty(userNames) ? userNames : "";
                                    }
                                    else if (dr["applytype"].ToString() == "1")
                                    {
                                        var parameter = new List<DbParameter>();
                                        //ȡ�ű�����ȡ�˻��ķ�Χ��Ϣ
                                        if (dr["scriptcurcontent"].ToString().Contains("@outengineerid"))
                                        {
                                            parameter.Add(DbParameters.CreateDbParameter("@outengineerid", !string.IsNullOrEmpty(project.ID) ? project.ID : ""));
                                        }
                                        DbParameter[] arrayparam = parameter.ToArray();
                                        string userIds = DbFactory.Base().FindList<UserEntity>(dr["scriptcurcontent"].ToString(), arrayparam).Cast<UserEntity>().Aggregate("", (current, user) => current + (user.UserId + ",")).Trim(',');
                                        DataTable users = userservice.GetUserTable(userIds.Split(','));
                                        string[] usernames = users.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                                        string[] deptnames = users.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                                        checkdata.auditdeptname = string.Join(",", deptnames);
                                        checkdata.auditusername = string.Join(",", usernames);
                                    }

                                    checkdata.auditremark = "";
                                    checkdata.isapprove = "0";
                                    if (isendflow)
                                        checkdata.isoperate = "0";
                                    else
                                        checkdata.isoperate = dr["id"].ToString() == flowid ? "1" : "0";
                                    nodelist.Add(checkdata);
                                }
                            }
                        }
                    }
                    else {
                        //��˼�¼
                        if (dr["auditdept"] != null && !string.IsNullOrEmpty(dr["auditdept"].ToString()))
                        {
                            CheckFlowData checkdata = new CheckFlowData();
                            DateTime auditdate;
                            DateTime.TryParse(dr["audittime"].ToString(), out auditdate);
                            checkdata.auditdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                            checkdata.auditdeptname = dr["auditdept"].ToString();
                            checkdata.auditusername = dr["auditpeople"].ToString();
                            checkdata.auditstate = dr["auditresult"].ToString() == "0" ? "ͬ��" : "��ͬ��";
                            checkdata.isapprove = "1";
                            checkdata.isoperate = "0";
                            checkdata.auditremark = dr["auditopinion"].ToString();
                            nodelist.Add(checkdata);
                        }
                        else
                        {
                            CheckFlowData checkdata = new CheckFlowData();
                            checkdata.auditdate = "";
                            //����,��Ա
                            var checkDeptId = dr["checkdeptid"].ToString();
                            if (dr["applytype"].ToString() == "0")
                            {
                                if (checkDeptId == "-1")
                                {
                                    var deptentity = new DepartmentService().GetEntity(project.ENGINEERLETDEPTID);
                                    if (deptentity != null)
                                    {
                                        checkDeptId = deptentity.DepartmentId;
                                        checkdata.auditdeptname = deptentity.FullName;
                                    }
                                    else
                                    {
                                        checkdata.auditdeptname = "��";
                                    }
                                }
                                else if (checkDeptId == "-2")
                                {
                                    var deptentity = new DepartmentService().GetEntity(project.OUTPROJECTID);
                                    if (deptentity != null)
                                    {
                                        checkDeptId = deptentity.DepartmentId;
                                        checkdata.auditdeptname = deptentity.FullName;
                                    }
                                    else
                                    {
                                        checkdata.auditdeptname = "��";
                                    }
                                }
                                else if (checkDeptId == "-3")
                                {
                                    var deptentity = new DepartmentService().GetEntity((until as DailyexamineEntity).CreateUserDeptId);
                                    if (deptentity != null)
                                    {
                                        checkDeptId = deptentity.DepartmentId;
                                        checkdata.auditdeptname = deptentity.FullName;
                                    }
                                    else
                                    {
                                        checkdata.auditdeptname = "��";
                                    }
                                }
                                else if (checkDeptId == "-6")
                                {
                                    if (!string.IsNullOrEmpty(project.SupervisorId))
                                    {
                                        var deptentity = new DepartmentService().GetEntity(project.SupervisorId);
                                        if (deptentity != null)
                                        {
                                            checkDeptId = deptentity.DepartmentId;
                                            checkdata.auditdeptname = deptentity.FullName;
                                        }
                                        else
                                        {
                                            checkdata.auditdeptname = "��";
                                        }
                                    }
                                    else
                                    {
                                        checkdata.auditdeptname = "��";
                                    }

                                }
                                else
                                {
                                    checkdata.auditdeptname = dr["checkdeptname"].ToString();
                                }
                                string userNames = new ScaffoldService().GetUserName(checkDeptId, dr["checkrolename"].ToString(), "0").Split('|')[0];
                                checkdata.auditusername = !string.IsNullOrEmpty(userNames) ? userNames : "";
                            }
                            else if (dr["applytype"].ToString() == "1")
                            {
                                var parameter = new List<DbParameter>();
                                //ȡ�ű�����ȡ�˻��ķ�Χ��Ϣ
                                if (dr["scriptcurcontent"].ToString().Contains("@outengineerid"))
                                {
                                    parameter.Add(DbParameters.CreateDbParameter("@outengineerid", !string.IsNullOrEmpty(project.ID) ? project.ID : ""));
                                }
                                if (dr["scriptcurcontent"].ToString().Contains("@engineerletdeptid")) //����ʲ���ݲ��Ų�ѯ�������ύ��ʱ��û��id���ű�ֻ�ܸ��ݲ������飩
                                {
                                    parameter.Add(DbParameters.CreateDbParameter("@engineerletdeptid", !string.IsNullOrEmpty(kbengineerletdeptid) ? kbengineerletdeptid : ""));
                                }
                                if (dr["scriptcurcontent"].ToString().Contains("@id"))
                                {
                                    parameter.Add(DbParameters.CreateDbParameter("@id", keyValue));
                                }
                                DbParameter[] arrayparam = parameter.ToArray();
                                string userIds = DbFactory.Base().FindList<UserEntity>(dr["scriptcurcontent"].ToString(), arrayparam).Cast<UserEntity>().Aggregate("", (current, user) => current + (user.UserId + ",")).Trim(',');
                                DataTable users = userservice.GetUserTable(userIds.Split(','));
                                string[] usernames = users.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                                string[] deptnames = users.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                                checkdata.auditdeptname = string.Join(",", deptnames);
                                checkdata.auditusername = string.Join(",", usernames);
                            }

                            checkdata.auditremark = "";
                            checkdata.isapprove = "0";
                            if (isendflow)
                                checkdata.isoperate = "0";
                            else
                                checkdata.isoperate = dr["id"].ToString() == flowid ? "1" : "0";
                            nodelist.Add(checkdata);
                        }
                    }
                       
                }
            }
            return nodelist;
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
        public void SaveForm(string keyValue, AptitudeinvestigateinfoEntity entity)
        {
            var res = DbFactory.Base().BeginTrans();
            entity.ID = keyValue;
            try
            {
                if (entity.ISSAVEORCOMMIT == "1")
                {
                    Operator currUser = OperatorProvider.Provider.Current();
                    string state = string.Empty;
                    ManyPowerCheckEntity nextCheck = review.CheckAuditPower(currUser, out state, "��λ�������", entity.OUTENGINEERID);
                    Repository<OutsouringengineerEntity> ourEngineer = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                    OutsouringengineerEntity engineerEntity = ourEngineer.FindEntity(entity.OUTENGINEERID);
                    string flowid = string.Empty;
                    if (!string.IsNullOrEmpty(state) && state == "1")
                    {
                        AptitudeinvestigateauditEntity auditEntity = new AptitudeinvestigateauditEntity();
                        auditEntity.ID = Guid.NewGuid().ToString();
                        auditEntity.APTITUDEID = entity.ID;
                        auditEntity.AUDITDEPT = currUser.DeptName;
                        auditEntity.AUDITDEPTID = currUser.DeptId;
                        auditEntity.AUDITRESULT = "0";
                        auditEntity.AUDITPEOPLEID = currUser.UserId;
                        auditEntity.AUDITPEOPLE = currUser.UserName;
                        auditEntity.AUDITTIME = DateTime.Now;
                        List<ManyPowerCheckEntity> powerList = new ManyPowerCheckService().GetListBySerialNum(currUser.OrganizeCode, "��λ�������");
                        List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
                        //�Ȳ��ִ�в��ű���
                        for (int i = 0; i < powerList.Count; i++)
                        {
                            if (powerList[i].CHECKDEPTCODE == "-1" || powerList[i].CHECKDEPTID == "-1")
                            {
                                powerList[i].CHECKDEPTCODE = new DepartmentService().GetEntity(engineerEntity.ENGINEERLETDEPTID).EnCode;
                                powerList[i].CHECKDEPTID = new DepartmentService().GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                            }
                            if (powerList[i].CHECKDEPTCODE == "-2" || powerList[i].CHECKDEPTID == "-2")
                            {
                                powerList[i].CHECKDEPTCODE = new DepartmentService().GetEntity(engineerEntity.OUTPROJECTID).EnCode;
                                powerList[i].CHECKDEPTID = new DepartmentService().GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                            }
                            if (powerList[i].CHECKDEPTCODE == "-6" || powerList[i].CHECKDEPTID == "-6")
                            {
                                if (!string.IsNullOrEmpty(engineerEntity.SupervisorId))
                                {
                                    powerList[i].CHECKDEPTCODE = new DepartmentService().GetEntity(engineerEntity.SupervisorId).EnCode;
                                    powerList[i].CHECKDEPTID = new DepartmentService().GetEntity(engineerEntity.SupervisorId).DepartmentId;
                                }
                                else
                                {
                                    powerList[i].CHECKDEPTCODE = "";
                                    powerList[i].CHECKDEPTID = "";
                                }
                            }
                        }
                        //��¼���Ƿ������Ȩ��--�����Ȩ��ֱ�����ͨ��
                        for (int i = 0; i < powerList.Count; i++)
                        {
                            if (powerList[i].ApplyType == "0")
                            {
                                if (powerList[i].CHECKDEPTID == currUser.DeptId)
                                {
                                    var rolelist = currUser.RoleName.Split(',');
                                    for (int j = 0; j < rolelist.Length; j++)
                                    {
                                        if (powerList[i].CHECKROLENAME.Contains(rolelist[j]))
                                        {
                                            checkPower.Add(powerList[i]);
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (powerList[i].ApplyType == "1")
                            {
                                var parameter = new List<DbParameter>();
                                //ȡ�ű�����ȡ�˻��ķ�Χ��Ϣ
                                if (powerList[i].ScriptCurcontent.Contains("@outengineerid"))
                                {
                                    parameter.Add(DbParameters.CreateDbParameter("@outengineerid", !string.IsNullOrEmpty(entity.OUTENGINEERID) ? entity.OUTENGINEERID : ""));
                                }
                                DbParameter[] arrayparam = parameter.ToArray();
                                var userIds = DbFactory.Base().FindList<UserEntity>(powerList[i].ScriptCurcontent,arrayparam).Cast<UserEntity>().Aggregate("", (current, user) => current + (user.Account + ",")).Trim(',');
                                if (userIds.Contains(currUser.UserId))
                                {
                                    checkPower.Add(powerList[i]);
                                    //break;
                                }
                            }
                        }
                        if (checkPower.Count > 0)
                        {
                            ManyPowerCheckEntity check = checkPower.Last();//��ǰ

                            for (int i = 0; i < powerList.Count; i++)
                            {
                                if (check.ID == powerList[i].ID)
                                {
                                    flowid = powerList[i].ID;
                                }
                            }
                        }
                        auditEntity.FlowId = flowid;
                        res.Insert<AptitudeinvestigateauditEntity>(auditEntity);
                    }
                    if (nextCheck == null)
                    {
                        entity.ISAUDITOVER = "1";
                        entity.FlowId = flowid;
                        //���¹�������״̬
                        Repository<StartappprocessstatusEntity> proecss = new Repository<StartappprocessstatusEntity>(DbFactory.Base());
                        StartappprocessstatusEntity startProecss = proecss.FindList(string.Format("select * from epg_startappprocessstatus t where t.outengineerid='{0}'", entity.OUTENGINEERID)).ToList().FirstOrDefault();
                        startProecss.EXAMSTATUS = "1";
                        res.Update<StartappprocessstatusEntity>(startProecss);
                        #region ͬ���������

                        engineerEntity.ENGINEERTECHPERSON = entity.ENGINEERTECHPERSON;
                        engineerEntity.ENGINEERWORKPEOPLE = entity.ENGINEERWORKPEOPLE;
                        engineerEntity.SAFEMANAGERPEOPLE = entity.SAFEMANAGERPEOPLE;
                        engineerEntity.ENGINEERDIRECTOR = entity.ENGINEERDIRECTOR;
                        engineerEntity.ENGINEERDIRECTORPHONE = entity.ENGINEERDIRECTORPHONE;
                        engineerEntity.ENGINEERCASHDEPOSIT = entity.ENGINEERCASHDEPOSIT;
                        engineerEntity.ENGINEERSCALE = entity.ENGINEERSCALE;
                        engineerEntity.ENGINEERCONTENT = entity.ENGINEERCONTENT;
                        engineerEntity.UnitSuper = entity.UnitSuper;
                        engineerEntity.UnitSuperId = entity.UnitSuperId;
                        engineerEntity.UnitSuperPhone = entity.UnitSuperPhone;

                        res.Update<OutsouringengineerEntity>(engineerEntity);
                        #endregion
                        #region ͬ�������λ
                        Repository<OutsourcingprojectEntity> ourProject = new Repository<OutsourcingprojectEntity>(DbFactory.Base());
                        OutsourcingprojectEntity projectEntity = ourProject.FindList(string.Format("select * from epg_outsourcingproject  t where t.OUTPROJECTID='{0}'", entity.OUTPROJECTID)).ToList().FirstOrDefault();
                        projectEntity.LEGALREP = entity.LEGALREP;
                        projectEntity.LEGALREPFAX = entity.LEGALREPFAX;
                        projectEntity.LEGALREPPHONE = entity.LEGALREPPHONE;
                        projectEntity.LINKMAN = entity.LINKMAN;
                        projectEntity.LINKMANFAX = entity.LINKMANFAX;
                        projectEntity.LINKMANPHONE = entity.LINKMANPHONE;

                        projectEntity.EMAIL = entity.EMAIL;
                        projectEntity.CREDITCODE = entity.CREDITCODE;
                        projectEntity.GENERALSITUATION = entity.GENERALSITUATION;
                        projectEntity.ADDRESS = entity.ADDRESS;
                        //�����볡״̬
                        if (string.IsNullOrWhiteSpace(projectEntity.OUTORIN))
                        {
                            projectEntity.OUTINTIME = DateTime.Now;
                            projectEntity.OUTORIN = "0";
                        }
                        //��������λΪ�볡״̬,����볡ʱ�䣬�������볡
                        else if (projectEntity.OUTORIN == "1")
                        {
                            projectEntity.LEAVETIME = null;
                            projectEntity.OUTINTIME = DateTime.Now;
                            projectEntity.OUTORIN = "0";
                        }
                        res.Update<OutsourcingprojectEntity>(projectEntity);
                        #endregion
                    }
                    else
                    {
                        entity.NEXTCHECKDEPTID = nextCheck.CHECKDEPTID;
                        entity.NEXTCHECKDEPTCODE = nextCheck.CHECKDEPTCODE;
                        entity.NEXTCHECKROLENAME = nextCheck.CHECKROLENAME;
                        entity.FlowId = nextCheck.ID;
                        entity.ISAUDITOVER = "0";
                    }
                }

                if (!string.IsNullOrEmpty(keyValue))
                {
                    //���ް�ȫ���֤ 1 �� �����ȫ���֤��Ϣ�͸�����Ϣ
                    if (entity.ISXK == "1")
                    {
                        entity.SPLCERTIFICATE = "";
                        entity.SPLCODE = "";
                        entity.SPLVALIDENDTIME = null;
                        entity.SPLVALIDSTARTTIME = null;
                        var recid = entity.ID + "02";
                        var file = new FileInfoService().GetFileList(recid);
                        if (file.Count > 0)
                        {
                            foreach (var item in file)
                            {
                                new FileInfoService().DeleteFile(recid, item.FileName, item.FilePath);
                            }

                        }
                    }
                    //��������֤�� 1 �� �������֤����Ϣ�͸�����Ϣ
                    if (entity.ISZZZJ == "1")
                    {
                        entity.CQCODE = "";
                        entity.CQLEVEL = "";
                        entity.CQORG = "";
                        entity.CQRANGE = "";
                        entity.CQVALIDENDTIME = null;
                        entity.CQVALIDSTARTTIME = null;
                        var recid = entity.ID + "03";
                        var file = new FileInfoService().GetFileList(recid);
                        if (file.Count > 0)
                        {
                            foreach (var item in file)
                            {
                                new FileInfoService().DeleteFile(recid, item.FileName, item.FilePath);
                            }

                        }
                    }
                    AptitudeinvestigateinfoEntity e = this.BaseRepository().FindEntity(keyValue);
                    if (e != null)
                    {
                        if (string.IsNullOrEmpty(entity.CREATEUSERID))
                        {
                            entity.Create();
                        }
                        entity.Modify(keyValue);
                        res.Update<AptitudeinvestigateinfoEntity>(entity);
                    }
                    else
                    {
                        entity.Create();
                        res.Insert<AptitudeinvestigateinfoEntity>(entity);
                    }
                }
                else
                {
                    entity.Create();
                    res.Insert<AptitudeinvestigateinfoEntity>(entity);
                }
                res.Commit();
            }
            catch (Exception)
            {

                res.Rollback();
            }

        }
        #endregion
    }
}
