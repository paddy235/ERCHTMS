using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data;
using BSFramework.Data;
using ERCHTMS.Code;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.HiddenTroubleManage;
using Newtonsoft.Json.Linq;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.SystemManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Service.OutsourcingProject;
using ERCHTMS.Entity.PublicInfoManage;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// �� ������ȫ��ʩ�䶯�����
    /// </summary>
    public class SafetychangeService : RepositoryFactory<SafetychangeEntity>, SafetychangeIService
    {
        private DataItemDetailService dataitemdetailservice = new DataItemDetailService();
        private IDepartmentService departmentIService = new DepartmentService();
        ScaffoldService scaffoldservice = new ScaffoldService();
        private HighRiskCommonApplyService highriskcommonapplyservice = new HighRiskCommonApplyService();
        private ManyPowerCheckService powerCheck = new ManyPowerCheckService();
        private UserService userservice = new UserService();
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafetychangeEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SafetychangeEntity GetEntity(string keyValue)
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
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                db.Delete<HighRiskRecordEntity>(t => t.WorkId.Equals(keyValue));
                db.Delete<FileInfoEntity>(t => t.RecId.Equals(keyValue));
                db.Delete<FileInfoEntity>(t => t.RecId.Equals(keyValue + "1"));
                db.Delete<SafetychangeEntity>(keyValue);
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
        public void SaveForm(string keyValue, SafetychangeEntity entity)
        {
            entity.ID = keyValue;
            var sl = BaseRepository().FindEntity(keyValue);
            if (sl == null)
            {
                entity.Create();
                string sql = string.Format("select id from BIS_SAFETYCHANGE t where createuserorgcode ='{0}'", ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeCode);
                if (string.IsNullOrWhiteSpace(entity.APPLYNO))
                {
                    int Code = this.BaseRepository().FindList(sql).ToList().Count;
                    var year = DateTime.Now.Year;
                    entity.APPLYNO = "BD" + year + "00" + (Code + 1);
                }
                this.BaseRepository().Insert(entity);
            }
            else
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
        }
        #endregion


        /// <summary>
        /// ��ȡ��ȫ��ʩ�䶯̨��
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetLedgerList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DatabaseType.Oracle;
            #region ����Ȩ��
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string curUserId = user.UserId;
            #endregion
            /*
             �����䶯:��ȫ��ʩ�䶯����ͨ������ʵ�ʱ䶯ʱ��Ϊ��
             �䶯��:��ȫ��ʩ�䶯����ͨ��ʵ�ʱ䶯ʱ�䲻Ϊ�գ���δ�䶯�ָ�����ͨ��
             �ѻָ�:��ȫ��ʩ�䶯�ָ�����ͨ��
             */
            #region ���
            pagination.p_kid = "t.Id as workid";
            pagination.p_fields = "applyno,workunit,case when workunittype='0'  then '��λ�ڲ�'  when  workunittype='1' then '�����λ' end workunittypename,changename,changetype,workplace,applychangetime,returntime,case when isaccpcommit=0 and isaccepover=0 and  realitychangetime is null then '�����䶯' when  isaccpcommit=1 and isaccepover=1 then '�ѻָ�'  else '�䶯��' end ledgertype,case when  isaccpcommit=1 and isaccepover=1 then  b.auditdate else null end checkdate,workfzrid,realitychangetime,t.createuserid,t.workunitid,t.workunittype,t.projectid,t.projectname,'' as isoperate";
            pagination.p_tablename = @" bis_Safetychange t left join
                    (
                    select
                        id,scaffoldid,auditusername,auditstate,auditdate,row_number() over(partition by scaffoldid order by auditdate desc) as num
                    from
                        bis_scaffoldauditrecord
                    ) b on t.id = b.scaffoldid and b.num = 1";
            pagination.conditionJson = "iscommit=1 and isapplyover=1";
            if (!user.IsSystem)
            {
                if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
                {
                    pagination.conditionJson += " and t.createuserorgcode='" + user.OrganizeCode + "'";
                }
                else
                {
                    pagination.conditionJson += string.Format(" and ((workunitcode in(select encode from base_department where encode like '{0}%'))  or (projectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                }
            }

            #endregion

            #region  ɸѡ����
            var queryParam = JObject.Parse(queryJson);
            if (!queryParam["changename"].IsEmpty())//��ȫ��ʩ�䶯����
            {
                pagination.conditionJson += string.Format("  and changename like '%{0}%'", queryParam["changename"].ToString());
            }
            //��ҵ��λ
            if (!queryParam["workunitcode"].IsEmpty() && !queryParam["workunitid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and ((workunitcode in(select encode from base_department where encode like '{0}%'))  or (projectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", queryParam["workunitcode"].ToString(), queryParam["workunitid"].ToString());
            }
            //ʱ��ѡ��
            if (!queryParam["st"].IsEmpty())//����䶯ʱ��
            {
                string from = queryParam["st"].ToString().Trim();
                pagination.conditionJson += string.Format(" and applytime>=to_date('{0}','yyyy-mm-dd')", from);

            }
            if (!queryParam["et"].IsEmpty())//Ԥ�ƻָ�ʱ��
            {
                string to = Convert.ToDateTime(queryParam["et"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and returntime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            if (!queryParam["keyname"].IsEmpty())//��ҵ��λ���ƻ�ȫ��ʩ�䶯����
            {
                pagination.conditionJson += string.Format("  and (changename like '%{0}%' or workunit like '%{0}%')", queryParam["keyname"].ToString());
            }
            if (!queryParam["ledgertype"].IsEmpty())
            {
                var ledgertype = queryParam["ledgertype"].ToString();
                if (ledgertype == "0")//�����䶯
                {
                    pagination.conditionJson += " and isaccpcommit=0 and isaccepover=0 and  RealityChangeTime is null";
                }
                else if (ledgertype == "1")//�䶯��
                {
                    pagination.conditionJson += " and t.id not in(select id from bis_safetychange where ((isaccpcommit=0 and isaccepover=0  and  RealityChangeTime is null) or (isaccpcommit=1 and isaccepover=1)))";
                }
                else//�ѻָ�
                {
                    pagination.conditionJson += " and isaccpcommit=1 and isaccepover=1";
                }
            }
            if (!queryParam["applynumber"].IsEmpty())
            {
                pagination.conditionJson += " and ApplyNo like '%" + queryParam["applynumber"] + "%'";
            }
            #endregion
            //return this.BaseRepository().FindTableByProcPager(pagination, dataType);
            var data = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            #region ����Ȩ��
            if (data != null)
            {
                string strRole = dataitemdetailservice.GetItemValue(user.OrganizeId, "LedgerSendDept");//�������Ž�ɫ
                string strManageRole = dataitemdetailservice.GetItemValue(user.OrganizeId, "LedgerManageDept");//��ȫ���ܲ��ż�ܽ�ɫ
                string strWorkRole = dataitemdetailservice.GetItemValue(user.OrganizeId, "LedgerWorkDept");//��ҵ��λ
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    string str = "0";
                    string dutyUserId = data.Rows[i]["workfzrid"].ToString();
                    string applyUserId = data.Rows[i]["createuserid"].ToString();
                    string projectid = data.Rows[i]["projectid"].ToString();//����id
                    string workDeptType = data.Rows[i]["workunittype"].ToString();
                    string workdeptid = data.Rows[i]["workunitid"].ToString();//��ҵ��λid

                    var dept = new OutsouringengineerService().GetEntity(projectid); //��ȡ����id��Ӧ�����β���
                    if (user.RoleName.Contains("����") && !string.IsNullOrEmpty(strManageRole))//��ȫ���ܲ���
                    {
                        string[] arrrolename = strManageRole.Split(',');
                        for (int j = 0; j < arrrolename.Length; j++)
                        {
                            if (user.RoleName.Contains(arrrolename[j]))
                            {
                                str = "1";
                                break;
                            }
                        }
                    }
                    if (str != "1" && !string.IsNullOrEmpty(workdeptid))
                    {
                        string[] arrrolename = strWorkRole.Split(',');
                        for (int j = 0; j < arrrolename.Length; j++)
                        {
                            if (user.RoleName.Contains(arrrolename[j]))
                            {
                                str = "1";
                                break;
                            }
                        }
                    }
                    if (str != "1" && (curUserId == dutyUserId || curUserId == applyUserId))//��ҵ�����˻�������
                    {
                        str = "1";
                    }
                    if (str != "1" && dept != null)
                    {
                        if (workDeptType == "1")//��������
                        {
                            if (dept.ENGINEERLETDEPTID == user.DeptId && !string.IsNullOrEmpty(strRole))
                            {
                                string[] arrrolename = strRole.Split(',');
                                for (int j = 0; j < arrrolename.Length; j++)
                                {
                                    if (user.RoleName.Contains(arrrolename[j]))
                                    {
                                        str = "1";
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    data.Rows[i]["isoperate"] = str;
                }
            }
            #endregion
            return data;
        }

        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var user = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["applyno"].IsEmpty())//������
            {
                pagination.conditionJson += string.Format(" and applyno like '%{0}%'", queryParam["applyno"].ToString());
            }
            //��ѯ����
            if (!queryParam["status"].IsEmpty())//��ҵ���״̬
            {
                switch (queryParam["status"].ToString())
                {
                    case "0":
                        pagination.conditionJson += string.Format(" and iscommit=0");//�䶯������ύ
                        break;
                    case "1":
                        pagination.conditionJson += string.Format(" and iscommit=1 and isapplyover=0");//�䶯��������
                        break;
                    case "2":
                        pagination.conditionJson += string.Format(" and iscommit=1 and isapplyover=2");//�䶯�������δͨ��
                        break;
                    case "3":
                        pagination.conditionJson += string.Format(" and  iscommit=1 and isapplyover=1 and isaccpcommit=0 and isaccepover=0");//�䶯����������ύ
                        break;
                    case "4":
                        pagination.conditionJson += string.Format(" and  iscommit=1 and isapplyover=1 and isaccpcommit=1 and isaccepover=0");//�䶯���մ����
                        break;
                    case "5":
                        pagination.conditionJson += string.Format(" and  iscommit=1 and isapplyover=1 and isaccpcommit=1 and isaccepover=2");//�䶯������˲�ͨ��
                        break;
                    case "6":
                        pagination.conditionJson += string.Format(" and  iscommit=1 and isapplyover=1 and isaccpcommit=1 and isaccepover=1");//�䶯�������ͨ��
                        break;

                    default:
                        break;
                }
                //pagination.conditionJson += string.Format(" and ApplyState='{0}'", queryParam["status"].ToString());
            }
            if (!queryParam["ApplyType"].IsEmpty())//��ҵ����
            {
                pagination.conditionJson += string.Format(" and ApplyType='{0}'", queryParam["ApplyType"].ToString());
            }
            //ʱ��ѡ��
            if (!queryParam["st"].IsEmpty())//��ҵ��ʼʱ��
            {
                string from = queryParam["st"].ToString().Trim();
                pagination.conditionJson += string.Format(" and applytime>=to_date('{0}','yyyy-mm-dd')", from);

            }
            if (!queryParam["et"].IsEmpty())//��ҵ����ʱ��
            {
                string to = Convert.ToDateTime(queryParam["et"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and applytime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            #region  �ල����������
            //�ල��������ҵ��λ
            if (!queryParam["taskdeptid"].IsEmpty())
            {
                if (queryParam["tasktype"].ToString() == "0")
                {
                    pagination.conditionJson += string.Format(" and workunitid='{0}'", queryParam["taskdeptid"].ToString());

                }
                else
                {
                    if (!queryParam["engineeringname"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(" and workunitid='{0}'", queryParam["taskdeptid"].ToString());
                    }
                    else
                    {
                        var depart = new DepartmentService().GetEntity(queryParam["taskdeptid"].ToString());
                        pagination.conditionJson += string.Format(" and ((workunitcode in(select encode from base_department where encode like '{0}%'))  or (projectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", depart.EnCode, depart.DepartmentId);

                    }
                }
            }
            if (!queryParam["engineeringname"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and projectname='{0}'", queryParam["engineeringname"].ToString());
            }
            #endregion
            if (!queryParam["myself"].IsEmpty())
            {
                if (queryParam["myself"].ToString() == "1")//��������(�κ�״̬)
                {
                    pagination.conditionJson += string.Format(" and t.createuserid='{0}'", user.UserId);
                }
                else if (queryParam["myself"].ToString() == "2")//���˴����(��)
                {

                    string strCondition = " and ((t.isapplyover =0 and  t.iscommit =1) or (t.isapplyover =1 and  t.iscommit =1 and  t.isaccpcommit =1 and t.isaccepover=0))";
                    DataTable dt = BaseRepository().FindTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson + strCondition);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string executedept = string.Empty;
                        highriskcommonapplyservice.GetExecutedept(dt.Rows[i]["workunittype"].ToString(), dt.Rows[i]["workunitid"].ToString(), dt.Rows[i]["projectid"].ToString(), out executedept);
                        string createdetpid = departmentIService.GetEntityByCode(dt.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentIService.GetEntityByCode(dt.Rows[i]["createuserdeptcode"].ToString()).DepartmentId;
                        string outsouringengineerdept = string.Empty;
                        highriskcommonapplyservice.GetOutsouringengineerDept(dt.Rows[i]["workunitid"].ToString(), out outsouringengineerdept);
                        string str = powerCheck.GetApproveUserAccount(dt.Rows[i]["nodeid"].ToString(), dt.Rows[i]["id"].ToString(), "", dt.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", "");
                        dt.Rows[i]["approveuseraccount"] = str;
                    }
                    string[] applyids = dt.Select("(outtransferuseraccount is null or outtransferuseraccount not like '%" + user.Account + ",%') and (approveuseraccount like '%" + user.Account + ",%' or intransferuseraccount like '%" + user.Account + ",%')").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();

                    pagination.conditionJson += string.Format(" and t.id in ('{0}') {1}", string.Join("','", applyids), strCondition);

                    ////�ҵ�
                    //string[] arrRole = user.RoleName.Split(',');
                    //string strWhere = string.Empty;
                    //foreach (string str in arrRole)
                    //{
                    //    //���
                    //    strWhere += string.Format(@"   select  distinct a.id from bis_safetychange a  where a.flowdept like'%{0}%' and a.flowrolename like '%{1}%' and  a.isapplyover =1 and  a.iscommit =1 and  a.isaccpcommit =1  union", user.DeptId, str);
                    //    //���
                    //    strWhere += string.Format(@"   select  distinct a.id from bis_safetychange a  where a.flowdept like'%{0}%' and a.flowrolename like '%{1}%' and  a.isapplyover =0 and  a.iscommit =1  union", user.DeptId, str);
                    //}
                    //strWhere = strWhere.Substring(0, strWhere.Length - 5);
                    //pagination.conditionJson += string.Format(" and t.id in ({0})", strWhere);
                }
            }
            else
            {
                //�ų����������뱣�������
                pagination.conditionJson += string.Format("  and t.id  not in(select id from bis_safetychange where iscommit='0' and  applypeopleid!='{0}')", user.UserId);
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }



        public Flow GetFlow(string keyValue, List<string> modulename)
        {
            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable nodeDt = GetCheckInfo(keyValue, modulename, user);
            var entity = this.GetEntity(keyValue);
            Flow flow = new Flow();
            flow.title = "";
            flow.initNum = 22;
            if (entity.ISCOMMIT == 0)
            {
                flow.activeID = entity.ID;
            }
            else
            {
                if (entity.ISACCPCOMMIT == 0 && entity.ISAPPLYOVER == 1)
                {
                    flow.activeID = entity.ID + "1";
                }
                else
                {
                    flow.activeID = entity.NodeId;
                }
            }
            if (nodeDt.Rows.Count > 0)
            {
                int num = 0;
                int cou = 0;
                for (int i = 0; i < nodeDt.Rows.Count; i++)
                {
                    DataRow dr = nodeDt.Rows[i];
                    nodes nodestart = new nodes();
                    //������Ӱ�ȫ��ʩ�䶯����ڵ�
                    if (i == 0)
                    {
                        nodestart.alt = true;
                        nodestart.isclick = false;
                        nodestart.css = "";
                        nodestart.id = entity.ID; //����
                        nodestart.img = "";
                        nodestart.name = "��ȫ��ʩ�䶯����";
                        nodestart.type = "stepnode";
                        nodestart.width = 150;
                        nodestart.height = 60;
                        //λ��
                        int m1 = num % 4;
                        int n1 = num / 4;
                        if (m1 == 0)
                        {
                            nodestart.left = 120;
                        }
                        else
                        {
                            nodestart.left = 120 + ((150 + 60) * m1);
                        }
                        if (n1 == 0)
                        {
                            nodestart.top = 54;
                        }
                        else
                        {
                            nodestart.top = (n1 * 100) + 54;
                        }
                        num++;
                    }
                    nodes nodemiddle = new nodes();
                    //������Ӱ�ȫ��ʩ��������ڵ�
                    if (nodeDt.Rows[i]["modulename"].ToString().Contains("����") && cou == 0)
                    {
                        nodemiddle.alt = true;
                        nodemiddle.isclick = false;
                        nodemiddle.css = "";
                        nodemiddle.id = entity.ID + "1"; //����
                        nodemiddle.img = "";
                        nodemiddle.name = "��ȫ��ʩ��������";
                        nodemiddle.type = "stepnode";
                        nodemiddle.width = 150;
                        nodemiddle.height = 60;
                        //λ��
                        int m1 = num % 4;
                        int n1 = num / 4;
                        if (m1 == 0)
                        {
                            nodemiddle.left = 120;
                        }
                        else
                        {
                            nodemiddle.left = 120 + ((150 + 60) * m1);
                        }
                        if (n1 == 0)
                        {
                            nodemiddle.top = 54;
                        }
                        else
                        {
                            nodemiddle.top = (n1 * 100) + 54;
                        }
                        num++;
                    }
                    nodes nodes = new nodes();
                    nodes.alt = true;
                    nodes.isclick = false;
                    nodes.css = "";
                    nodes.id = dr["id"].ToString(); //����
                    nodes.img = "";
                    nodes.name = dr["flowname"].ToString();
                    nodes.type = "stepnode";
                    nodes.width = 150;
                    nodes.height = 60;
                    //λ��
                    int m = num % 4;
                    int n = num / 4;
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

                    if (i == 0)
                    {
                        setInfo sinfostart = new setInfo();
                        sinfostart.NodeName = nodestart.name;
                        if (entity.ISCOMMIT == 0)
                        {
                            sinfostart.Taged = 0;
                        }
                        else
                        {
                            sinfostart.Taged = 1;
                        }
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        DateTime auditdate;
                        DateTime.TryParse(dr["auditdate"].ToString(), out auditdate);
                        nodedesignatedata.createdate = Convert.ToDateTime(entity.APPLYTIME).ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.creatdept = entity.APPLYUNIT;
                        nodedesignatedata.createuser = entity.APPLYPEOPLE;
                        nodedesignatedata.status = entity.ISCOMMIT == 0 ? "������" : "������";
                        nodedesignatedata.prevnode = "��";
                        nodelist.Add(nodedesignatedata);
                        sinfostart.NodeDesignateData = nodelist;
                        nodestart.setInfo = sinfostart;
                        nlist.Add(nodestart);
                    }
                    if (nodeDt.Rows[i]["modulename"].ToString().Contains("����") && cou == 0)
                    {
                        if (string.IsNullOrEmpty(entity.ACCEPDEPT))
                            break;
                        setInfo sinfomiddle = new setInfo();
                        sinfomiddle.NodeName = nodemiddle.name;
                        if (entity.ISACCPCOMMIT == 0 && entity.ISAPPLYOVER == 1)
                        {
                            sinfomiddle.Taged = 0;
                        }
                        else
                        {
                            if (entity.ISACCPCOMMIT == 1)
                            {
                                sinfomiddle.Taged = 1;
                            }
                        }
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        DateTime auditdate;
                        DateTime.TryParse(dr["auditdate"].ToString(), out auditdate);
                        nodedesignatedata.createdate = Convert.ToDateTime(entity.ACCEPTIME).ToString("yyyy-MM-dd HH:mm") == "0001-01-01 00:00" ? "��" : Convert.ToDateTime(entity.ACCEPTIME).ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.creatdept = !string.IsNullOrEmpty(entity.ACCEPDEPT) ? entity.ACCEPDEPT : "��";
                        nodedesignatedata.createuser = !string.IsNullOrEmpty(entity.ACCEPPEOPLE) ? entity.ACCEPPEOPLE : "��";
                        if (entity.ISCOMMIT == 0)
                        {
                            nodedesignatedata.status = "��";
                        }
                        else
                        {
                            if (entity.ISACCPCOMMIT == 0)
                                nodedesignatedata.status = "������";
                            else
                                nodedesignatedata.status = "������";
                        }
                        nodedesignatedata.prevnode = nodeDt.Rows[i - 1]["flowname"].ToString();
                        nodelist.Add(nodedesignatedata);
                        sinfomiddle.NodeDesignateData = nodelist;
                        nodemiddle.setInfo = sinfomiddle;
                        nlist.Add(nodemiddle);
                        cou++;
                    }
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes.name;
                    //��˼�¼
                    if (dr["auditdeptname"] != null && !string.IsNullOrEmpty(dr["auditdeptname"].ToString()))
                    {
                        sinfo.Taged = 1;
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        DateTime auditdate;
                        DateTime.TryParse(dr["auditdate"].ToString(), out auditdate);
                        nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.creatdept = dr["auditdeptname"].ToString();
                        nodedesignatedata.createuser = dr["auditusername"].ToString();
                        nodedesignatedata.status = dr["auditstate"].ToString() == "0" ? "ͬ��" : "��ͬ��";
                        if (i == 0)
                        {
                            nodedesignatedata.prevnode = "��ȫ��ʩ�䶯����";
                        }
                        else
                        {
                            if (nodeDt.Rows[i]["modulename"].ToString().Contains("����") && cou == 1)
                            {
                                nodedesignatedata.prevnode = "��ȫ��ʩ��������";
                                cou++;
                            }
                            else
                            {
                                nodedesignatedata.prevnode = nodeDt.Rows[i - 1]["flowname"].ToString();
                            }
                        }
                        nodelist.Add(nodedesignatedata);
                        sinfo.NodeDesignateData = nodelist;
                        nodes.setInfo = sinfo;
                    }
                    else
                    {
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        nodedesignatedata.createdate = "��";
                        string executedept = string.Empty;
                        highriskcommonapplyservice.GetExecutedept(entity.WORKUNITTYPE, entity.WORKUNITID, entity.PROJECTID, out executedept);//��ȡִ�в���
                        string createdetpid = departmentIService.GetEntityByCode(entity.CREATEUSERDEPTCODE).IsEmpty() ? "" : departmentIService.GetEntityByCode(entity.CREATEUSERDEPTCODE).DepartmentId; //��ȡ��������ID
                        string outsouringengineerdept = string.Empty;
                        highriskcommonapplyservice.GetOutsouringengineerDept(entity.WORKUNITID, out outsouringengineerdept);
                        string accountstr = powerCheck.GetApproveUserAccount(dr["id"].ToString(), entity.ID, "", entity.SPECIALTYTYPE, executedept, outsouringengineerdept, createdetpid, "", ""); //��ȡ������˺�
                        string outtransferuseraccount = dr["outtransferuseraccount"].IsEmpty() ? "" : dr["outtransferuseraccount"].ToString();//ת��������
                        string intransferuseraccount = dr["intransferuseraccount"].IsEmpty() ? "" : dr["intransferuseraccount"].ToString();//ת��������
                        string[] outtransferuseraccountlist = outtransferuseraccount.Split(',');
                        string[] intransferuseraccountlist = intransferuseraccount.Split(',');
                        foreach (var item in intransferuseraccountlist)
                        {
                            if (!item.IsEmpty() && !accountstr.Contains(item + ","))
                            {
                                accountstr += (item + ",");//��ת�������˼�������˺���
                            }
                        }
                        foreach (var item in outtransferuseraccountlist)
                        {
                            if (!item.IsEmpty() && accountstr.Contains(item + ","))
                            {
                                accountstr = accountstr.Replace(item + ",", "");//��ת�������˴�����˺����Ƴ�
                            }
                        }

                        DataTable dtuser = userservice.GetUserTable(accountstr.Split(','));
                        string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                        string[] deptnames = dtuser.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                        nodedesignatedata.createuser = usernames.Length > 0 ? string.Join(",", usernames) : "��";
                        nodedesignatedata.creatdept = deptnames.Length > 0 ? string.Join(",", deptnames) : "��";

                        nodedesignatedata.status = "��";
                        if (i == 0)
                        {
                            nodedesignatedata.prevnode = "��ȫ��ʩ�䶯����";
                        }
                        else
                        {
                            if (nodeDt.Rows[i]["modulename"].ToString().Contains("����") && cou == 1)
                            {
                                nodedesignatedata.prevnode = "��ȫ��ʩ��������";
                                cou++;
                            }
                            else
                            {
                                nodedesignatedata.prevnode = nodeDt.Rows[i - 1]["flowname"].ToString();
                            }
                        }

                        nodelist.Add(nodedesignatedata);
                        sinfo.NodeDesignateData = nodelist;
                        nodes.setInfo = sinfo;
                    }
                    nlist.Add(nodes);
                    num++;
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
                if ((entity.ISCOMMIT == 1 && entity.ISAPPLYOVER == 2) || (entity.ISCOMMIT == 1 && entity.ISAPPLYOVER == 1 && entity.ISACCPCOMMIT == 1 && entity.ISACCEPOVER == 2)
                || (entity.ISCOMMIT == 1 && entity.ISAPPLYOVER == 1 && entity.ISACCPCOMMIT == 1 && entity.ISACCEPOVER == 1))
                {
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes_end.name;
                    sinfo.Taged = 1;
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    NodeDesignateData nodedesignatedata = new NodeDesignateData();

                    //ȡ���̽���ʱ�Ľڵ���Ϣ
                    DataRow[] end_rows = nodeDt.Select("auditusername is not null");
                    DataRow end_row = end_rows[end_rows.Count() - 1];
                    DateTime auditdate;
                    DateTime.TryParse(end_row["auditdate"].ToString(), out auditdate);
                    nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                    nodedesignatedata.creatdept = end_row["auditdeptname"].ToString();
                    nodedesignatedata.createuser = end_row["checkrolename"].ToString();
                    nodedesignatedata.status = end_row["auditstate"].ToString() == "0" ? "ͬ��" : "��ͬ��";
                    nodedesignatedata.prevnode = end_row["flowname"].ToString();

                    nodelist.Add(nodedesignatedata);
                    sinfo.NodeDesignateData = nodelist;
                    nodes_end.setInfo = sinfo;
                }
                int count = 0;
                for (int i = 0; i < nodeDt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        lines linestart = new lines();
                        linestart.alt = true;
                        linestart.id = Guid.NewGuid().ToString();
                        linestart.from = entity.ID;
                        if (i < nodeDt.Rows.Count - 1)
                        {
                            linestart.to = nodeDt.Rows[i]["id"].ToString();
                        }
                        linestart.name = "";
                        linestart.type = "sl";
                        llist.Add(linestart);
                    }
                    if (nodeDt.Rows[i]["modulename"].ToString().Contains("����") && count == 0)
                    {
                        if (string.IsNullOrEmpty(entity.ACCEPDEPT))
                            break;
                        lines linemiddle = new lines();
                        linemiddle.alt = true;
                        linemiddle.id = Guid.NewGuid().ToString();
                        linemiddle.from = entity.ID + "1";
                        linemiddle.to = nodeDt.Rows[i]["id"].ToString();
                        linemiddle.name = "";
                        linemiddle.type = "sl";
                        llist.Add(linemiddle);
                        count += 1;
                    }
                    lines lines = new lines();
                    lines.alt = true;
                    lines.id = Guid.NewGuid().ToString();
                    lines.from = nodeDt.Rows[i]["id"].ToString();
                    if (i < nodeDt.Rows.Count - 1)
                    {
                        if (GetCount(user, entity.ID, modulename[0]) == (i + 1))
                        {
                            lines.to = entity.ID + "1";
                        }
                        else
                        {
                            lines.to = nodeDt.Rows[i + 1]["id"].ToString();
                        }
                    }
                    lines.name = "";
                    lines.type = "sl";
                    llist.Add(lines);
                }
                lines lines_end = new lines();
                lines_end.alt = true;
                lines_end.id = Guid.NewGuid().ToString();
                //��δ��������,�����������������
                if (!string.IsNullOrEmpty(entity.ACCEPDEPT))
                {
                    lines_end.from = nodeDt.Rows[nodeDt.Rows.Count - 1]["id"].ToString();
                }
                else
                {
                    lines_end.from = nodeDt.Rows[GetCount(user, entity.ID, modulename[0]) - 1]["id"].ToString();
                }
                lines_end.to = nodes_end.id;
                llist.Add(lines_end);

                flow.nodes = nlist;
                flow.lines = llist;
            }


            return flow;
        }

        private DataTable GetCheckInfo(string keyValue, List<string> modulename, Operator user)
        {
            string node_sql = string.Empty;
            for (int i = 0; i < modulename.Count; i++)
            {

                node_sql += string.Format(@"  select * from(select 
a.id,a.flowname,a.serialnum,a.autoid,a.checkdeptname,a.checkroleid,a.checkrolename,a.checkdeptid,a.remark,a.modulename,b.auditdeptname,b.auditusername,b.auditdate,b.auditstate,b.auditremark,e.outtransferuseraccount,e.intransferuseraccount
                                              from 
                                                    bis_manypowercheck a left join bis_scaffoldauditrecord b
                                                    on a.id = b.flowid and b.scaffoldid = '{2}'
                                                    left join(select recid,flowid,outtransferuseraccount,intransferuseraccount,row_number() over(partition by recid,flowid order by createdate desc) as num from bis_transferrecord where disable=0 ) e on a.id=e.flowid and e.recid='{2}' and e.num=1
                                              where 
                                                    a.createuserorgcode = {0} and a.modulename = '{1}'
                                              order by
                                                    serialnum) union all", user.OrganizeCode, modulename[i], keyValue);

            }
            if (!string.IsNullOrEmpty(node_sql))
            {
                node_sql = node_sql.Substring(0, node_sql.Length - 9);
            }
            DataTable nodeDt = this.BaseRepository().FindTable(node_sql);
            return nodeDt;
        }

        //��ȡ�䶯������˵Ĳ�����
        private int GetCount(Operator user, string keyValue, string modelname)
        {
            string sql = string.Format("select count(1) num from   bis_manypowercheck a left join bis_scaffoldauditrecord b on a.id = b.flowid and b.scaffoldid = '{0}' where a.createuserorgcode ='{1}' and a.modulename like '%{2}%'", keyValue, user.OrganizeCode, modelname);
            int count = Convert.ToInt32(this.BaseRepository().FindTable(sql).Rows[0][0].ToString());
            return count;
        }
        public List<CheckFlowData> GetAppFlowList(string keyValue, List<string> modulename, string flowid, bool isendflow, string workdeptid, string projectid, string specialtytype = "")
        {
            List<CheckFlowData> nodelist = new List<CheckFlowData>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable dt = GetCheckInfo(keyValue, modulename, user);
            var entity = this.GetEntity(keyValue);
            if (dt != null)
            {
                int count = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        CheckFlowData checkdata = new CheckFlowData();
                        checkdata.auditdate = Convert.ToDateTime(entity.APPLYTIME).ToString("yyyy-MM-dd HH:mm") == "0001-01-01 00:00" ? "" : Convert.ToDateTime(entity.APPLYTIME).ToString("yyyy-MM-dd HH:mm");
                        checkdata.auditdeptname = !string.IsNullOrEmpty(entity.APPLYUNIT) ? entity.APPLYUNIT : "";
                        checkdata.auditusername = !string.IsNullOrEmpty(entity.APPLYPEOPLE) ? entity.APPLYPEOPLE : "";
                        checkdata.auditstate = entity.ISCOMMIT == 0 ? "������" : "������";
                        checkdata.auditremark = "";
                        checkdata.isapprove = entity.ISCOMMIT == 0 ? "0" : "1";
                        checkdata.isoperate = entity.ISCOMMIT == 0 ? "1" : "0";
                        nodelist.Add(checkdata);
                    }
                    if (dt.Rows[i]["modulename"].ToString().Contains("����") && count == 0)
                    {
                        if (string.IsNullOrEmpty(entity.ACCEPDEPT))
                            break;
                        CheckFlowData checkdata = new CheckFlowData();
                        checkdata.auditdate = Convert.ToDateTime(entity.ACCEPTIME).ToString("yyyy-MM-dd HH:mm") == "0001-01-01 00:00" ? "" : Convert.ToDateTime(entity.ACCEPTIME).ToString("yyyy-MM-dd HH:mm");
                        checkdata.auditdeptname = !string.IsNullOrEmpty(entity.ACCEPDEPT) ? entity.ACCEPDEPT : "";
                        checkdata.auditusername = !string.IsNullOrEmpty(entity.ACCEPPEOPLE) ? entity.ACCEPPEOPLE : "";
                        if (entity.ISCOMMIT == 0)
                        {
                            checkdata.auditstate = "";
                        }
                        else
                        {
                            if (entity.ISACCPCOMMIT == 0)
                                checkdata.auditstate = "������";
                            else
                                checkdata.auditstate = "������";
                        }
                        checkdata.auditremark = "";
                        checkdata.isapprove = entity.ISACCPCOMMIT == 1 ? "1" : "0";
                        checkdata.isoperate = (entity.ISACCPCOMMIT == 0 && entity.ISAPPLYOVER == 1) ? "1" : "0";
                        nodelist.Add(checkdata);
                        count++;
                    }
                    DataRow dr = dt.Rows[i];
                    //��˼�¼
                    if (dr["auditdeptname"] != null && !string.IsNullOrEmpty(dr["auditdeptname"].ToString()))
                    {

                        CheckFlowData checkdata = new CheckFlowData();
                        DateTime auditdate;
                        DateTime.TryParse(dr["auditdate"].ToString(), out auditdate);
                        checkdata.auditdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        checkdata.auditdeptname = dr["auditdeptname"].ToString();
                        checkdata.auditusername = dr["auditusername"].ToString();
                        checkdata.auditstate = dr["auditstate"].ToString() == "0" ? "ͬ��" : "��ͬ��";
                        checkdata.auditremark = dr["auditremark"].ToString();
                        checkdata.isapprove = "1";
                        checkdata.isoperate = "0";
                        nodelist.Add(checkdata);
                    }
                    else
                    {
                        CheckFlowData checkdata = new CheckFlowData();
                        checkdata.auditdate = "";

                        string executedept = string.Empty;
                        highriskcommonapplyservice.GetExecutedept(entity.WORKUNITTYPE, entity.WORKUNITID, entity.PROJECTID, out executedept);//��ȡִ�в���
                        string createdetpid = departmentIService.GetEntityByCode(entity.CREATEUSERDEPTCODE).IsEmpty() ? "" : departmentIService.GetEntityByCode(entity.CREATEUSERDEPTCODE).DepartmentId; //��ȡ��������ID
                        string outsouringengineerdept = string.Empty;
                        highriskcommonapplyservice.GetOutsouringengineerDept(entity.WORKUNITID, out outsouringengineerdept);
                        string accountstr = powerCheck.GetApproveUserAccount(dr["id"].ToString(), entity.ID, "", entity.SPECIALTYTYPE, executedept, outsouringengineerdept, createdetpid, "", ""); //��ȡ������˺�
                        string outtransferuseraccount = dr["outtransferuseraccount"].IsEmpty() ? "" : dr["outtransferuseraccount"].ToString();//ת��������
                        string intransferuseraccount = dr["intransferuseraccount"].IsEmpty() ? "" : dr["intransferuseraccount"].ToString();//ת��������
                        string[] outtransferuseraccountlist = outtransferuseraccount.Split(',');
                        string[] intransferuseraccountlist = intransferuseraccount.Split(',');
                        foreach (var item in intransferuseraccountlist)
                        {
                            if (!item.IsEmpty() && !accountstr.Contains(item + ","))
                            {
                                accountstr += (item + ",");//��ת�������˼�������˺���
                            }
                        }
                        foreach (var item in outtransferuseraccountlist)
                        {
                            if (!item.IsEmpty() && accountstr.Contains(item + ","))
                            {
                                accountstr = accountstr.Replace(item + ",", "");//��ת�������˴�����˺����Ƴ�
                            }
                        }

                        DataTable dtuser = userservice.GetUserTable(accountstr.Split(','));
                        string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                        string[] deptnames = dtuser.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                        checkdata.auditusername = usernames.Length > 0 ? string.Join(",", usernames) : "��";
                        checkdata.auditdeptname = deptnames.Length > 0 ? string.Join(",", deptnames) : "��";
                        ////����,��Ա
                        //var checkDeptId = dr["checkdeptid"].ToString();
                        //var checkremark = dr["remark"].ToString();
                        //string type = checkremark != "1" ? "0" : "1";
                        //if (checkDeptId == "-1" || checkDeptId == "-2")
                        //{
                        //    var deptentity = scaffoldservice.GetDutyDept(workdeptid, projectid, checkDeptId);
                        //    if (deptentity != null)
                        //    {
                        //        checkDeptId = deptentity.DepartmentId;
                        //        checkdata.auditdeptname = deptentity.FullName;
                        //    }
                        //    else
                        //    {
                        //        checkdata.auditdeptname = "";
                        //    }
                        //}
                        //else
                        //{
                        //    checkdata.auditdeptname = dr["checkdeptname"].ToString();
                        //}
                        //string userNames = scaffoldservice.GetUserName(checkDeptId, dr["checkrolename"].ToString(), type, specialtytype).Split('|')[0];
                        //checkdata.auditusername = !string.IsNullOrEmpty(userNames) ? userNames : "";
                        checkdata.auditremark = "";
                        checkdata.isapprove = "0";
                        if (isendflow)
                        {
                            checkdata.isoperate = "0";
                        }
                        else
                        {
                            if (entity.ISACCPCOMMIT == 0 && entity.ISAPPLYOVER == 1)
                            {
                                checkdata.isoperate = "0";
                            }
                            else
                            {
                                checkdata.isoperate = dr["id"].ToString() == flowid ? "1" : "0";
                            }
                        }
                        if (checkdata.isoperate == "1")
                        {
                            if (dr["flowname"].ToString().Contains("ȷ��"))
                            {
                                checkdata.auditstate = "ȷ����";
                            }
                            else
                            {
                                checkdata.auditstate = "���(��)��";
                            }
                        }
                        else
                        {
                            checkdata.auditstate = "";
                        }
                        nodelist.Add(checkdata);
                    }
                }
            }
            return nodelist;
        }
        public DataTable FindTable(string sql)
        {
            return this.BaseRepository().FindTable(sql);
        }
    }
}
