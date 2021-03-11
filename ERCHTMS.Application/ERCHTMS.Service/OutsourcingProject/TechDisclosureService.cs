using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using System.Web;
using Newtonsoft.Json;
using ERCHTMS.Service.PublicInfoManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// �� ������ȫ��������
    /// </summary>
    public class TechDisclosureService : RepositoryFactory<TechDisclosureEntity>, TechDisclosureIService
    {
        private PeopleReviewService peoplereviewservice = new PeopleReviewService();
        private AptitudeinvestigateauditService aptitudeinvestigateauditservice = new AptitudeinvestigateauditService();
        private HistoryTechDisclosureService historytechdisclosureservice = new HistoryTechDisclosureService();
        private FileInfoService fileinfoservice = new FileInfoService();
        private OutsouringengineerService outsouringengineerservice = new OutsouringengineerService();
        private DepartmentService departmentservice = new DepartmentService();
        private ManyPowerCheckService powerCheck = new ManyPowerCheckService();
        #region ��ȡ����
        public IEnumerable<TechDisclosureEntity> GetList() {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            //ʱ�䷶Χ
            if (!queryParam["sTime"].IsEmpty() || !queryParam["eTime"].IsEmpty())
            {
                string startTime = queryParam["sTime"].ToString();
                string endTime = queryParam["eTime"].ToString();
                if (queryParam["sTime"].IsEmpty())
                {
                    startTime = "1899-01-01";
                }
                if (queryParam["eTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                pagination.conditionJson += string.Format(" and to_date(to_char(disclosuredate,'yyyy-MM-dd'),'yyyy-MM-dd') between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            //��ѯ����
            if (!queryParam["condition"].IsEmpty() && !queryParam["txtSearch"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and {0} like '%{1}%'", queryParam["condition"].ToString(), queryParam["txtSearch"].ToString());
            }
            if (!queryParam["outengineerid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.projectid ='{0}' ", queryParam["outengineerid"].ToString());
            }
            if (!queryParam["unitid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and e.departmentid ='{0}' ", queryParam["unitid"].ToString());
            }
            if (!queryParam["deptid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.engineerletdept ='{0}' ", queryParam["deptid"].ToString());
            }
            if (!queryParam["orgCode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.createuserorgcode ='{0}' ", queryParam["orgCode"].ToString());
            }
            if (!queryParam["indexState"].IsEmpty())//��ҳ����
            {
                string strCondition = "";
                strCondition = string.Format(" and t.createuserorgcode='{0}' and t.issubmit=1 and t.status=1 ", user.OrganizeCode);
                DataTable data = BaseRepository().FindTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson + strCondition);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var engineerEntity = outsouringengineerservice.GetEntity(data.Rows[i]["engineerid"].ToString());
                    var excutdept = engineerEntity == null ? "" : departmentservice.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                    var outengineerdept = engineerEntity == null ? "" : departmentservice.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                    var supervisordept = engineerEntity == null ? "" : string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentservice.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                    //��ȡ��һ�������
                    string str = powerCheck.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, data.Rows[i]["engineerid"].ToString());
                    data.Rows[i]["approveuserids"] = str;
                }

                string[] applyids = data.Select(" approveuserids like '%" + user.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();

                pagination.conditionJson += string.Format(" and t.id in ('{0}') {1}", string.Join("','", applyids), strCondition);
            }
            if (!queryParam["projectid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and r.id ='{0}'", queryParam["projectid"].ToString());
            }
            if (!queryParam["disclosuremajor"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.disclosuremajor ='{0}'", queryParam["disclosuremajor"].ToString());
            }
            if (!queryParam["disclosuremajordeptid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.disclosuremajordeptid='{0}'", queryParam["disclosuremajordeptid"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TechDisclosureEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        public DataTable GetNameByPorjectId(string projectId, string type) {
            string sql = string.Format(@"select * from EPG_TECHDISCLOSURE t where t.DISCLOSURETYPE='{0}' and t.projectid='{1}'",type,projectId);
            return this.BaseRepository().FindTable(sql);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                TechDisclosureEntity se = this.BaseRepository().FindEntity(keyValue);
                this.BaseRepository().Delete(keyValue);
                string sql = string.Format(@"select t.id from EPG_TechDisclosure t where t.ProjectID='{0}' and t.status=3", se.PROJECTID);
                DataTable dt = this.BaseRepository().FindTable(sql);
                if (dt == null || dt.Rows.Count <= 0)
                {
                    #region ���¹�������״̬
                    Repository<StartappprocessstatusEntity> proecss = new Repository<StartappprocessstatusEntity>(DbFactory.Base());
                    StartappprocessstatusEntity startProecss = proecss.FindList(string.Format("select * from epg_startappprocessstatus t where t.outengineerid='{0}'", se.PROJECTID)).ToList().FirstOrDefault();
                    startProecss.TECHNICALSTATUS = "0";
                    res.Update<StartappprocessstatusEntity>(startProecss);
                    res.Commit();
                    #endregion
                }
            }
            catch (System.Exception)
            {
                res.Rollback();
            }
            
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, TechDisclosureEntity entity)
        {
            entity.ID = keyValue;
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                //�����ֻ��˽�������
                if (string.IsNullOrWhiteSpace(entity.DISCLOSURENAME)) {
                    DataTable dt = GetNameByPorjectId(entity.PROJECTID, entity.DISCLOSURETYPE);
                    var num = dt.Rows.Count;
                    var no = string.Empty;
                    switch (num.ToString().Length)
                    {
                        case 1:
                            no = "00" + (num + 1);
                            break;
                        case 2:
                            no = "0" + (num + 1);
                            break;
                        case 3:
                            no = (num + 1).ToString();
                            break;
                        default:
                            break;
                    }
                    //entity.DISCLOSURENAME = new OutsouringengineerService().GetEntity(entity.PROJECTID).ENGINEERNAME + entity.DISCLOSURETYPE + DateTime.Now.ToString("yyyyMMdd") + no;
                    entity.DISCLOSURENAME = entity.ENGINEERNAME + entity.DISCLOSURETYPE + DateTime.Now.ToString("yyyyMMdd") + no;
                }
                if (entity.ISSUBMIT == 0)
                {
                    entity.STATUS = 0;
                }
                else
                {
                    string modulename = "";
                    if (string.IsNullOrWhiteSpace(entity.PROJECTID))
                    {
                        modulename = "��ȫ��������(���乤��)";
                        if (entity.ENGINEERLEVEL == "001")
                        {
                            modulename += "_һ������";
                        }
                        else if (entity.ENGINEERLEVEL == "002")
                        {
                            modulename += "_��������";
                        }
                        else if (entity.ENGINEERLEVEL == "003")
                        {
                            modulename += "_��������";
                        }
                    }
                    else
                    {
                        modulename = "��ȫ��������(ѡ�񹤳�)";
                        if (entity.ENGINEERLEVEL == "001")
                        {
                            modulename += "_һ������";
                        }
                        else if (entity.ENGINEERLEVEL == "002")
                        {
                            modulename += "_��������";
                        }
                        else if (entity.ENGINEERLEVEL == "003")
                        {
                            modulename += "_��������";
                        }
                    }
                    string state = string.Empty;
                    ManyPowerCheckEntity mpcEntity = peoplereviewservice.CheckAuditPower(curUser, out state, modulename, entity.PROJECTID, false);
                    if (mpcEntity == null)
                    {
                        entity.STATUS = 3;
                    }
                    else
                    {
                        entity.STATUS = 1;
                        entity.FLOWID = mpcEntity.ID;
                    }
                }
                entity.DISCLOSUREMAJORNAME = null;
                entity.ENGINEERLEVELNAME = null;
                entity.ENGINEERTYPENAME = null;
                if (!string.IsNullOrEmpty(keyValue))
                {
                    TechDisclosureEntity se = this.BaseRepository().FindEntity(keyValue);
                    if (se == null)
                    {
                        entity.Create();
                        res.Insert<TechDisclosureEntity>(entity);
                       
                    }
                    else
                    {
                        entity.Modify(keyValue);
                        res.Update<TechDisclosureEntity>(entity);
                    }
                }
                else
                {
                    entity.Create();
                    res.Insert<TechDisclosureEntity>(entity);
                }
                #region ���¹�������״̬
                if (entity.STATUS == 3 && entity.PROJECTID !="" && entity.PROJECTID !=null)
                {
                    Repository<StartappprocessstatusEntity> proecss = new Repository<StartappprocessstatusEntity>(DbFactory.Base());
                    StartappprocessstatusEntity startProecss = proecss.FindList(string.Format("select * from epg_startappprocessstatus t where t.outengineerid='{0}'", entity.PROJECTID)).ToList().FirstOrDefault();
                    startProecss.TECHNICALSTATUS = "1";
                    res.Update<StartappprocessstatusEntity>(startProecss);
                }
                #endregion
                res.Commit();
            }
            catch (System.Exception)
            {
                res.Rollback();
            }
           
        }

        /// <summary>
        /// ��˱�
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="aentity"></param>
        public void ApporveForm(string keyValue, TechDisclosureEntity entity, AptitudeinvestigateauditEntity aentity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var smEntity = GetEntity(keyValue);
            try
            {
                string state = string.Empty;
                string moduleName = "";
                if (string.IsNullOrWhiteSpace(smEntity.PROJECTID))
                {
                    moduleName = "��ȫ��������(���乤��)";
                    if (smEntity.ENGINEERLEVEL == "001")
                    {
                        moduleName += "_һ������";
                    }
                    else if (smEntity.ENGINEERLEVEL == "002")
                    {
                        moduleName += "_��������";
                    }
                    else if (smEntity.ENGINEERLEVEL == "003")
                    {
                        moduleName += "_��������";
                    }
                }
                else
                {
                    moduleName = "��ȫ��������(ѡ�񹤳�)";
                    if (smEntity.ENGINEERLEVEL == "001")
                    {
                        moduleName += "_һ������";
                    }
                    else if (smEntity.ENGINEERLEVEL == "002")
                    {
                        moduleName += "_��������";
                    }
                    else if (smEntity.ENGINEERLEVEL == "003")
                    {
                        moduleName += "_��������";
                    }
                }

                string outengineerid = smEntity.PROJECTID;
                ManyPowerCheckEntity mpcEntity = peoplereviewservice.CheckAuditPower(curUser, out state, moduleName, outengineerid, false, smEntity.FLOWID);


                #region //�����Ϣ��
                AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
                aidEntity.AUDITRESULT = aentity.AUDITRESULT; //ͨ��
                aidEntity.AUDITTIME = Convert.ToDateTime(aentity.AUDITTIME.Value.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss")); //���ʱ��
                aidEntity.AUDITPEOPLE = aentity.AUDITPEOPLE;  //�����Ա����
                aidEntity.AUDITPEOPLEID = aentity.AUDITPEOPLEID;//�����Աid
                aidEntity.APTITUDEID = keyValue;  //������ҵ��ID 
                aidEntity.AUDITDEPTID = aentity.AUDITDEPTID;//��˲���id
                aidEntity.AUDITDEPT = aentity.AUDITDEPT; //��˲���
                aidEntity.AUDITOPINION = aentity.AUDITOPINION; //������
                aidEntity.FlowId = smEntity.FLOWID;
                aentity.AUDITSIGNIMG = HttpUtility.UrlDecode(aentity.AUDITSIGNIMG);
                aidEntity.AUDITSIGNIMG = string.IsNullOrWhiteSpace(aentity.AUDITSIGNIMG) ? "" : aentity.AUDITSIGNIMG.ToString().Replace("../..", "");
                if (null != mpcEntity)
                {
                    aidEntity.REMARK = (mpcEntity.AUTOID.Value - 1).ToString(); //��ע �����̵�˳���
                }
                else
                {
                    aidEntity.REMARK = "7";
                }
                aptitudeinvestigateauditservice.SaveForm(aidEntity.ID, aidEntity);
                #endregion

                #region  //���氲ȫ��������
                
                //���ͨ��
                if (aentity.AUDITRESULT == "0")
                {
                    //0��ʾ����δ��ɣ�1��ʾ���̽���
                    if (null != mpcEntity)
                    {
                        smEntity.STATUS = 1;
                        smEntity.FLOWID = mpcEntity.ID;
                    }
                    else
                    {
                        smEntity.STATUS = 3;
                        smEntity.FLOWID = "";
                    }
                }
                else //��˲�ͨ�� 
                {
                    smEntity.ISSUBMIT = 0;
                    smEntity.STATUS = 0;
                    smEntity.FLOWID = "";

                }
                
                #endregion

                #region    //��˲�ͨ��
                if (aentity.AUDITRESULT == "1")
                {
                    //�����ʷ��¼
                    HistoryTechDisclosureEntity hsentity = new HistoryTechDisclosureEntity();
                    hsentity = JsonConvert.DeserializeObject<HistoryTechDisclosureEntity>(JsonConvert.SerializeObject(smEntity));
                    hsentity.ID = "";
                    hsentity.RecId = smEntity.ID;
                    historytechdisclosureservice.SaveForm(hsentity.ID, hsentity);

                    //��ȡ��ǰҵ������������˼�¼
                    var shlist = aptitudeinvestigateauditservice.GetAuditList(keyValue);
                    //����������˼�¼����ID
                    foreach (AptitudeinvestigateauditEntity mode in shlist)
                    {
                        mode.APTITUDEID = hsentity.ID; //��Ӧ�µ�ID
                        aptitudeinvestigateauditservice.SaveForm(mode.ID, mode);
                    }
                    //�������¸�����¼����ID
                    var flist = fileinfoservice.GetImageListByObject(keyValue);
                    foreach (FileInfoEntity fmode in flist)
                    {
                        fmode.RecId = hsentity.ID; //��Ӧ�µ�ID
                        fileinfoservice.SaveForm("", fmode);
                    }
                }
                #endregion
                //���°�ȫ�������׻���״̬��Ϣ
                smEntity.Modify(keyValue);
                this.BaseRepository().Update(smEntity);
                #region ���¹�������״̬
                if (smEntity.STATUS == 3 && smEntity.PROJECTID != "" && smEntity.PROJECTID != null)
                {
                    Repository<StartappprocessstatusEntity> proecss = new Repository<StartappprocessstatusEntity>(DbFactory.Base());
                    StartappprocessstatusEntity startProecss = proecss.FindList(string.Format("select * from epg_startappprocessstatus t where t.outengineerid='{0}'", smEntity.PROJECTID)).ToList().FirstOrDefault();
                    startProecss.TECHNICALSTATUS = "1";
                    var res = DbFactory.Base().BeginTrans();
                    res.Update<StartappprocessstatusEntity>(startProecss);
                }
                #endregion
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        #endregion
    }
}
