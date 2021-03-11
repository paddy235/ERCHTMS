using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using ERCHTMS.Service.BaseManage;
using System.Data;
using BSFramework.Util;
using Newtonsoft.Json;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.IService.PublicInfoManage;
using ERCHTMS.Service.PublicInfoManage;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using System.Data.Common;
using ERCHTMS.Service.SystemManage;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// �� �������������˱�
    /// </summary>
    public class AptitudeinvestigateauditService : RepositoryFactory<AptitudeinvestigateauditEntity>, AptitudeinvestigateauditIService
    {
        IFileInfoService fileService = new FileInfoService();
        PeopleReviewIService peopleReview = new PeopleReviewService();
        UserService userservice = new UserService();
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<AptitudeinvestigateauditEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        //public List<AptitudeinvestigateauditEntity> GetAuditList(string keyValue)
        //{
        //    string sql = string.Format(@"select * from epg_aptitudeinvestigateaudit t where t.aptitudeid='{0}'", keyValue);
        //    return new RepositoryFactory().BaseRepository().FindList<AptitudeinvestigateauditEntity>(sql).ToList();
        //}
        public DataTable GetAuditRecList(string keyValue)
        {
            string sql = string.Format(@"select auditpeople,auditpeopleid,auditresult,audittime,auditopinion,auditdept,createtime,auditdeptid,aptitudeid,auditsignimg,flowid from epg_aptitudeinvestigateaudit t where t.aptitudeid='{0}' order by createtime asc", keyValue);
            return new RepositoryFactory().BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public AptitudeinvestigateauditEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        public AptitudeinvestigateauditEntity GetAuditEntity(string FKId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select * from epg_aptitudeinvestigateaudit t where t.APTITUDEID='{0}'", FKId);
            return new RepositoryFactory().BaseRepository().FindList<AptitudeinvestigateauditEntity>(strSql.ToString()).ToList().FirstOrDefault();
        }

        public List<AptitudeinvestigateauditEntity> GetAuditList(string keyValue)
        {
            string sql = string.Format(@"select * from epg_aptitudeinvestigateaudit t where t.aptitudeid='{0}' order by createtime", keyValue);
            return new RepositoryFactory().BaseRepository().FindList<AptitudeinvestigateauditEntity>(sql).ToList();
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["aptitudeid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.aptitudeid='{0}' ", queryParam["aptitudeid"].ToString());
            }
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
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
        /// ����ҵ������ɾ������
        /// </summary>
        /// <param name="aptitudeId"></param>
        public void DeleteFormByAptitudeId(string aptitudeId)
        {
            string sql = "delete from EPG_APTITUDEINVESTIGATEAUDIT where APTITUDEID=:APTITUDEID";
            DbParameter[] dbParameters = {
                 DbParameters.CreateDbParameter(":APTITUDEID", aptitudeId),
            };
            this.BaseRepository().ExecuteBySql(sql, dbParameters);
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, AptitudeinvestigateauditEntity entity)
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
        /// ��������������޸ģ�
        /// ����������ͨ��ͬ�� ���� ��λ ��Ա��Ϣ
        /// �޸������λ�볡״̬,�޸����̱��������״̬
        /// ͬʱ����һ����ȫ��֤��,��ͬ,Э������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        public void SaveSynchrodata(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {

                Repository<AptitudeinvestigateinfoEntity> repAudit = new Repository<AptitudeinvestigateinfoEntity>(DbFactory.Base());
                AptitudeinvestigateinfoEntity auditInfo = repAudit.FindEntity(entity.APTITUDEID);
                //���ͨ��
                if (entity.AUDITRESULT == "0")
                {
                    Operator currUser = OperatorProvider.Provider.Current();
                    string state = string.Empty;
                    ManyPowerCheckEntity nextCheck = peopleReview.CheckAuditPower(currUser, out state, "��λ�������", auditInfo.OUTENGINEERID);
                    if (nextCheck == null)
                    {
                        auditInfo.ISAUDITOVER = "1";
                        #region ͬ���������
                        Repository<OutsouringengineerEntity> ourEngineer = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                        OutsouringengineerEntity engineerEntity = ourEngineer.FindEntity(auditInfo.OUTENGINEERID);
                        engineerEntity.ENGINEERTECHPERSON = auditInfo.ENGINEERTECHPERSON;
                        engineerEntity.ENGINEERWORKPEOPLE = auditInfo.ENGINEERWORKPEOPLE;
                        engineerEntity.SAFEMANAGERPEOPLE = auditInfo.SAFEMANAGERPEOPLE;
                        engineerEntity.ENGINEERDIRECTOR = auditInfo.ENGINEERDIRECTOR;
                        engineerEntity.ENGINEERDIRECTORPHONE = auditInfo.ENGINEERDIRECTORPHONE;
                        engineerEntity.ENGINEERCASHDEPOSIT = auditInfo.ENGINEERCASHDEPOSIT;
                        engineerEntity.ENGINEERSCALE = auditInfo.ENGINEERSCALE;
                        engineerEntity.ENGINEERCONTENT = auditInfo.ENGINEERCONTENT;
                        engineerEntity.UnitSuper = auditInfo.UnitSuper;
                        engineerEntity.UnitSuperId = auditInfo.UnitSuperId;
                        engineerEntity.UnitSuperPhone = auditInfo.UnitSuperPhone;

                        res.Update<OutsouringengineerEntity>(engineerEntity);
                        #endregion
                        #region ͬ�������λ
                        Repository<OutsourcingprojectEntity> ourProject = new Repository<OutsourcingprojectEntity>(DbFactory.Base());
                        OutsourcingprojectEntity projectEntity = ourProject.FindList(string.Format("select * from epg_outsourcingproject  t where t.OUTPROJECTID='{0}'", auditInfo.OUTPROJECTID)).ToList().FirstOrDefault();
                        projectEntity.LEGALREP = auditInfo.LEGALREP;
                        projectEntity.LEGALREPFAX = auditInfo.LEGALREPFAX;
                        projectEntity.LEGALREPPHONE = auditInfo.LEGALREPPHONE;
                        projectEntity.LINKMAN = auditInfo.LINKMAN;
                        projectEntity.LINKMANFAX = auditInfo.LINKMANFAX;
                        projectEntity.LINKMANPHONE = auditInfo.LINKMANPHONE;
                        //projectEntity.OUTORIN = "0";
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
                        projectEntity.EMAIL = auditInfo.EMAIL;
                        projectEntity.CREDITCODE = auditInfo.CREDITCODE;
                        projectEntity.GENERALSITUATION = auditInfo.GENERALSITUATION;
                        projectEntity.ADDRESS = auditInfo.ADDRESS;
                        res.Update<OutsourcingprojectEntity>(projectEntity);
                        #endregion
                        #region ���¹�������״̬
                        Repository<StartappprocessstatusEntity> proecss = new Repository<StartappprocessstatusEntity>(DbFactory.Base());
                        StartappprocessstatusEntity startProecss = proecss.FindList(string.Format("select * from epg_startappprocessstatus t where t.outengineerid='{0}'", auditInfo.OUTENGINEERID)).ToList().FirstOrDefault();
                        startProecss.EXAMSTATUS = "1";
                        res.Update<StartappprocessstatusEntity>(startProecss);
                        #endregion
                    }
                    else
                    {
                        auditInfo.NEXTCHECKROLENAME = nextCheck.CHECKROLENAME;
                        auditInfo.NEXTCHECKDEPTID = nextCheck.CHECKDEPTID;
                        auditInfo.NEXTCHECKDEPTCODE = nextCheck.CHECKDEPTCODE;
                        auditInfo.FlowId = nextCheck.ID;
                        auditInfo.ISAUDITOVER = "0";
                    }
                    res.Update<AptitudeinvestigateinfoEntity>(auditInfo);
                }
                else
                {
                    #region ��˲�ͨ�������ʷ��¼��Ϣ
                    #region ���ʼ�¼--������ʷ��¼
                    var str = JsonConvert.SerializeObject(auditInfo);
                    HistoryRecordDetail hrd = JsonConvert.DeserializeObject<HistoryRecordDetail>(str);
                    hrd.ID = Guid.NewGuid().ToString();
                    res.Insert<HistoryRecordDetail>(hrd);
                    //ͬ��������Ϣ����---������ʷ��¼����
                    for (int i = 1; i <= 10; i++)
                    {
                        var id = auditInfo.ID + "0" + i;
                        var file1 = fileService.GetFiles(id);
                        if (file1.Rows.Count > 0)
                        {
                            var key = hrd.ID + "0" + i;
                            foreach (DataRow item in file1.Rows)
                            {
                                FileInfoEntity itemFile = new FileInfoEntity();
                                itemFile.FileName = item["FileName"].ToString();
                                itemFile.FilePath = item["filepath"].ToString();
                                itemFile.FileSize = item["filesize"].ToString();
                                itemFile.RecId = key;
                                fileService.SaveForm(itemFile.FileId, itemFile);
                            }
                        }
                    }
                    #endregion
                    #region ����һ����ʷ��¼
                    HistoryRecord his_record = new HistoryRecord();
                    his_record.ID = Guid.NewGuid().ToString();
                    his_record.HistoryzzscId = hrd.ID;
                    //his_record.HistoryauditId = his_audit.ID;
                    his_record.APPLYPEOPLEID = auditInfo.CREATEUSERID;
                    his_record.APPLYPEOPLE = auditInfo.CREATEUSERNAME;
                    his_record.APPLYDATE = auditInfo.CREATEDATE;
                    his_record.Zzscid = auditInfo.ID;
                    his_record.Create();
                    res.Insert<HistoryRecord>(his_record);
                    #endregion
                    #region ��˼�¼--������ʷ��¼
                    entity.APTITUDEID = his_record.ID;
                    Repository<AptitudeinvestigateauditEntity> audit = new Repository<AptitudeinvestigateauditEntity>(DbFactory.Base());
                    List<AptitudeinvestigateauditEntity> list = audit.FindList(string.Format(@"select * from  EPG_APTITUDEINVESTIGATEAUDIT t where t.APTITUDEID='{0}'", auditInfo.ID)).ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].APTITUDEID = his_record.ID;
                    }
                    res.Update<AptitudeinvestigateauditEntity>(list);
                    #endregion
                    #region ����֤����ʷ��¼
                     Repository<QualificationEntity> QuaRep = new Repository<QualificationEntity>(DbFactory.Base());
                     List<QualificationEntity> QuaList = QuaRep.FindList(string.Format(@"select * from  epg_qualification t where t.infoid='{0}'", auditInfo.ID)).ToList();
                     for (int i = 0; i < QuaList.Count; i++)
                     {
                         var file1 = fileService.GetFiles(QuaList[i].ID);
                         QuaList[i].ID = Guid.NewGuid().ToString();
                         QuaList[i].Create();
                         QuaList[i].InfoId = hrd.ID;
                         res.Insert<QualificationEntity>(QuaList);
                         if (file1.Rows.Count > 0)
                         {
                             var key = QuaList[i].ID;
                             foreach (DataRow item in file1.Rows)
                             {
                                 FileInfoEntity itemFile = new FileInfoEntity();
                                 itemFile.FileName = item["FileName"].ToString();
                                 itemFile.FilePath = item["filepath"].ToString();
                                 itemFile.FileSize = item["filesize"].ToString();
                                 itemFile.RecId = key;
                                 fileService.SaveForm(itemFile.FileId, itemFile);
                             }
                         }
                     }
               
                    #endregion
                    #endregion
                    //��˲�ͨ���޸��ύ״̬
                    auditInfo.ISSAVEORCOMMIT = "0";
                    auditInfo.NEXTCHECKDEPTCODE = "";
                    auditInfo.NEXTCHECKDEPTID = "";
                    auditInfo.NEXTCHECKROLENAME = "";
                    auditInfo.FlowId = "";
                    res.Update<AptitudeinvestigateinfoEntity>(auditInfo);
                }
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    res.Update<AptitudeinvestigateauditEntity>(entity);
                }
                else
                {
                    entity.Create();
                    res.Insert<AptitudeinvestigateauditEntity>(entity);
                }
                res.Commit();
            }
            catch (System.Exception ex)
            {

                res.Rollback();
            }

        }
        /// <summary>
        /// ��������������޸ģ�
        /// �����������:���¹���״̬
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void AuditReturnForWork(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    res.Update<AptitudeinvestigateauditEntity>(entity);
                }
                else
                {
                    entity.Create();
                    res.Insert<AptitudeinvestigateauditEntity>(entity);
                }
                Repository<ReturntoworkEntity> returnwork = new Repository<ReturntoworkEntity>(DbFactory.Base());
                ReturntoworkEntity returnworkEntity = returnwork.FindEntity(entity.APTITUDEID);
                if (entity.AUDITRESULT == "0")
                {

                    Repository<OutsouringengineerEntity> ourEngineer = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                    OutsouringengineerEntity engineerEntity = ourEngineer.FindEntity(returnworkEntity.OUTENGINEERID);
                    engineerEntity.STOPRETURNSTATE = "0";
                    res.Update<OutsouringengineerEntity>(engineerEntity);
                }
                else
                {
                    #region ��˲�ͨ�������ʷ��¼��Ϣ
                    var str = JsonConvert.SerializeObject(returnworkEntity);
                    HisReturnWorkEntity hisreturn = JsonConvert.DeserializeObject<HisReturnWorkEntity>(str);
                    hisreturn.ID = Guid.NewGuid().ToString();
                    Repository<OutsourcingprojectEntity> ourProject = new Repository<OutsourcingprojectEntity>(DbFactory.Base());
                    OutsourcingprojectEntity projectEntity = ourProject.FindList(string.Format("select * from epg_outsourcingproject  t where t.OUTPROJECTID='{0}'", returnworkEntity.OUTPROJECTID)).ToList().FirstOrDefault();
                    hisreturn.OUTPROJECT = projectEntity.OUTSOURCINGNAME;
                    hisreturn.RETURNID = returnworkEntity.ID;
                    res.Insert<HisReturnWorkEntity>(hisreturn);

                    //ͬ������
                    var file1 = fileService.GetFiles(returnworkEntity.ID);
                    if (file1.Rows.Count > 0)
                    {
                        var key = hisreturn.ID;
                        foreach (DataRow item in file1.Rows)
                        {
                            FileInfoEntity itemFile = new FileInfoEntity();
                            itemFile.FileName = item["FileName"].ToString();
                            itemFile.FilePath = item["filepath"].ToString();
                            itemFile.FileSize = item["filesize"].ToString();
                            itemFile.RecId = key;
                            fileService.SaveForm(itemFile.FileId, itemFile);
                        }
                    }
                    //���������ʷ��Ϣ
                    var str_audit = JsonConvert.SerializeObject(entity);
                    HistoryAudit his_audit = JsonConvert.DeserializeObject<HistoryAudit>(str_audit);
                    his_audit.ID = Guid.NewGuid().ToString();
                    his_audit.APTITUDEID = hisreturn.ID;
                    res.Insert<HistoryAudit>(his_audit);
                    #endregion

                    returnworkEntity.ISCOMMIT = "0";
                    res.Update<ReturntoworkEntity>(returnworkEntity);
                }
                res.Commit();
            }
            catch (Exception)
            {

                res.Rollback();
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// ��ȫ��֤�����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveSafetyEamestMoney(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    res.Update<AptitudeinvestigateauditEntity>(entity);
                }
                else
                {
                    entity.Create();
                    res.Insert<AptitudeinvestigateauditEntity>(entity);
                }
                Repository<SafetyEamestMoneyEntity> repAudit = new Repository<SafetyEamestMoneyEntity>(DbFactory.Base());
                SafetyEamestMoneyEntity auditInfo = repAudit.FindEntity(entity.APTITUDEID);
                if (entity.AUDITRESULT == "0")
                {

                    #region ���¹�������״̬
                    Repository<StartappprocessstatusEntity> proecss = new Repository<StartappprocessstatusEntity>(DbFactory.Base());
                    StartappprocessstatusEntity startProecss = proecss.FindList(string.Format("select * from epg_startappprocessstatus t where t.outengineerid='{0}'", auditInfo.PROJECTID)).ToList().FirstOrDefault();
                    startProecss.SECURITYSTATUS = "1";
                    res.Update<StartappprocessstatusEntity>(startProecss);
                    #endregion
                }
                else
                {
                    //��˲�ͨ�������ʷ��¼��Ϣ
                    var str = JsonConvert.SerializeObject(auditInfo);
                    HistoryMoneyEntity hisMoney = JsonConvert.DeserializeObject<HistoryMoneyEntity>(str);
                    hisMoney.ID = Guid.NewGuid().ToString();
                    hisMoney.MONEYID = auditInfo.ID;
                    res.Insert<HistoryMoneyEntity>(hisMoney);
                    Repository<SafetyMoneyExamineEntity> examine = new Repository<SafetyMoneyExamineEntity>(DbFactory.Base());
                    var list_examine = examine.IQueryable().Where(x => x.SafetymoneyId == auditInfo.ID).ToList();
                    for (int i = 0; i < list_examine.Count; i++)
                    {
                        list_examine[i].Id = Guid.NewGuid().ToString();
                        list_examine[i].Create();
                        list_examine[i].SafetymoneyId = hisMoney.ID;
                    }
                    res.Insert<SafetyMoneyExamineEntity>(list_examine);
                    //ͬ������
                    var file1 = fileService.GetFiles(auditInfo.ID);
                    if (file1.Rows.Count > 0)
                    {
                        var key = hisMoney.ID;
                        foreach (DataRow item in file1.Rows)
                        {
                            FileInfoEntity itemFile = new FileInfoEntity();
                            itemFile.FileName = item["FileName"].ToString();
                            itemFile.FilePath = item["filepath"].ToString();
                            itemFile.FileSize = item["filesize"].ToString();
                            itemFile.RecId = key;
                            fileService.SaveForm(itemFile.FileId, itemFile);
                        }
                    }
                    //���������ʷ��Ϣ
                    var str_audit = JsonConvert.SerializeObject(entity);
                    HistoryAudit his_audit = JsonConvert.DeserializeObject<HistoryAudit>(str_audit);
                    his_audit.ID = Guid.NewGuid().ToString();
                    his_audit.APTITUDEID = hisMoney.ID;
                    res.Insert<HistoryAudit>(his_audit);
                }
                res.Commit();
            }
            catch (Exception)
            {
                res.Rollback();
            }

        }
        /// <summary>
        /// ��������������޸ģ�
        /// �����������:���¹���״̬
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void AuditStartApply(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    res.Update<AptitudeinvestigateauditEntity>(entity);
                }
                else
                {
                    entity.Create();
                    res.Insert<AptitudeinvestigateauditEntity>(entity);
                }
                Repository<StartapplyforEntity> startApply = new Repository<StartapplyforEntity>(DbFactory.Base());
                StartapplyforEntity startApplyEntity = startApply.FindEntity(entity.APTITUDEID);
                if (entity.AUDITRESULT == "0")
                {

                    Repository<OutsouringengineerEntity> ourEngineer = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                    OutsouringengineerEntity engineerEntity = ourEngineer.FindEntity(startApplyEntity.OUTENGINEERID);
                    engineerEntity.ENGINEERSTATE = "002";//�����������ͨ�����¹���״̬Ϊ�ڽ�
                    engineerEntity.PLANENDDATE = startApplyEntity.APPLYRETURNTIME;
                    res.Update<OutsouringengineerEntity>(engineerEntity);
                    #region ��ȫ����
                    //SafetyEvaluateEntity see = new SafetyEvaluateEntity();
                    //see.ID = Guid.NewGuid().ToString();
                    //see.PROJECTID = engineerEntity.ID;
                    //see.Create();
                    //res.Insert<SafetyEvaluateEntity>(see);
                    #endregion
                }
                else
                {
                    #region ��˲�ͨ�������ʷ��¼��Ϣ
                    var str = JsonConvert.SerializeObject(startApplyEntity);
                    HistoryStartapplyEntity hisapply = JsonConvert.DeserializeObject<HistoryStartapplyEntity>(str);
                    hisapply.ID = Guid.NewGuid().ToString();
                    Repository<OutsourcingprojectEntity> ourProject = new Repository<OutsourcingprojectEntity>(DbFactory.Base());
                    OutsourcingprojectEntity projectEntity = ourProject.FindList(string.Format("select * from epg_outsourcingproject  t where t.OUTPROJECTID='{0}'", startApplyEntity.OUTPROJECTID)).ToList().FirstOrDefault();
                    hisapply.OUTPROJECT = projectEntity.OUTSOURCINGNAME;
                    hisapply.APPLYID = startApplyEntity.ID;
                    res.Insert<HistoryStartapplyEntity>(hisapply);
                    //ͬ������
                    var file1 = fileService.GetFiles(startApplyEntity.ID);
                    if (file1.Rows.Count > 0)
                    {
                        var key = hisapply.ID;
                        foreach (DataRow item in file1.Rows)
                        {
                            FileInfoEntity itemFile = new FileInfoEntity();
                            itemFile.FileName = item["FileName"].ToString();
                            itemFile.FilePath = item["filepath"].ToString();
                            itemFile.FileSize = item["filesize"].ToString();
                            itemFile.RecId = key;
                            fileService.SaveForm(itemFile.FileId, itemFile);
                        }
                    }
                    //���������ʷ��Ϣ
                    var str_audit = JsonConvert.SerializeObject(entity);
                    HistoryAudit his_audit = JsonConvert.DeserializeObject<HistoryAudit>(str_audit);
                    his_audit.ID = Guid.NewGuid().ToString();
                    his_audit.APTITUDEID = hisapply.ID;
                    res.Insert<HistoryAudit>(his_audit);
                    #endregion
                    //��˲�ͨ����Ҫ�����ύ
                    startApplyEntity.ISCOMMIT = "0";
                    startApplyEntity.IsOver = 0;
                    res.Update<StartapplyforEntity>(startApplyEntity);
                }
                res.Commit();
            }
            catch (Exception)
            {

                res.Rollback();
            }
        }
        /// <summary>
        /// ��Ա�������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public List<string> AuditPeopleReview(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            Repository<PeopleReviewEntity> rep = new Repository<PeopleReviewEntity>(DbFactory.Base());
            PeopleReviewEntity p_entity = rep.FindEntity(keyValue);
            Repository<OutsouringengineerEntity> repEng = new Repository<OutsouringengineerEntity>(DbFactory.Base());
            OutsouringengineerEntity outeng = repEng.FindEntity(p_entity.OUTENGINEERID);
            Repository<AptitudeinvestigatepeopleEntity> reppeople = new Repository<AptitudeinvestigatepeopleEntity>(DbFactory.Base());
            List<AptitudeinvestigatepeopleEntity> peoplelist = reppeople.FindList(string.Format("select * from epg_aptitudeinvestigatepeople t where t.PEOPLEREVIEWID='{0}'", keyValue)).ToList();
            List<UserEntity> UserEntitylist = new List<UserEntity>();
            List<string> userids = new List<string>();
            try
            {
                if (entity.AUDITRESULT == "0")
                {
                    Operator currUser = OperatorProvider.Provider.Current();
                    string state = string.Empty;
                    ManyPowerCheckEntity nextCheck = peopleReview.CheckAuditPower(currUser, out state, "��Ա�������", p_entity.OUTENGINEERID);
                    if (nextCheck == null)
                    {
                        p_entity.ISAUDITOVER = "1";
                        //���¹�������״̬
                        Repository<StartappprocessstatusEntity> proecss = new Repository<StartappprocessstatusEntity>(DbFactory.Base());
                        StartappprocessstatusEntity startProecss = proecss.FindList(string.Format("select * from epg_startappprocessstatus t where t.outengineerid='{0}'", p_entity.OUTENGINEERID)).ToList().FirstOrDefault();
                        startProecss.PEOPLESTATUS = "1";
                        res.Update<StartappprocessstatusEntity>(startProecss);

                        Repository<UserEntity> repuser = new Repository<UserEntity>(DbFactory.Base());
                        //List<UserEntity> list = new List<UserEntity>();
                        //��Ա���������˽���ͬ����Ա��Ϣ
                        foreach (var item in peoplelist)
                        {
                            var expression = LinqExtensions.True<UserEntity>();
                            expression = expression.And(t => t.IdentifyID == item.IDENTIFYID && t.Account == item.ACCOUNTS);
                            var user = res.FindList<UserEntity>(expression).FirstOrDefault();
                            if (user == null)
                            {
                                #region ����ͬ����Ա��֤��
                                string pwd = "Abc123456";
                                string key = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();
                                pwd = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(pwd, 32).ToLower(), key).ToLower(), 32).ToLower();

                                string sql = "";
                                string isldap = GetItemValue("IsOpenPassword");
                                if (isldap == "true")
                                {
                                    pwd = "Abcd1234";
                                    pwd = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(pwd, 32).ToLower(), key).ToLower(), 32).ToLower();
                                }
                                //ͬ����Ա��Ϣ
                                sql = string.Format(@"insert into base_user(
                                                        userid,encode,realname,headicon,birthday,mobile,telephone,email,oicq,wechat,
                                                        msn,organizeid,dutyid,dutyname,postid,postname,gender,isspecialequ,isspecial,
                                                        projectid,nation,native,usertype,isepiboly,degreesid,degrees,identifyid,
                                                        organizecode,createuserdeptcode,createuserorgcode,createdate,createuserid,
                                                        departmentid,departmentcode,ISPRESENCE,account,enabledmark,password,secretkey,
                                                        craft,deletemark,entertime,isfourperson,fourpersontype,healthstatus,craftage,ACCOUNTTYPE,ISAPPLICATIONLDAP,quickquery,simplespelling,managerid,
                                                        manager,district,districtcode,street,address,age,SpecialtyType)
                                               select p.id,p.encode,p.realname,p.headicon,p.birthday,p.mobile,p.telephone,p.email,
                                                        p.oicq,p.wechat,p.msn,p.organizeid,p.dutyid,p.dutyname,p.postid,p.postname,
                                                        p.gender,p.isspecialequ,p.isspecial,p.OUTENGINEERID,p.nation,p.native,p.usertype,
                                                        p.isepiboly,p.degreesid,p.degrees,p.identifyid,p.organizecode,p.createuserdeptcode,
                                                        p.createuserorgcode,p.createdate,p.createuserid,p.outprojectid,p.outprojectcode,'1',
                                                        accounts as account,1,'{1}','{2}',p.workoftype,0,to_date('{3}','yyyy-MM-dd'),
                                                        isfourperson,fourpersontype,stateofhealth,workyear,ACCOUNTTYPE,ISAPPLICATIONLDAP,quickquery,simplespelling,managerid,
                                                        manager,district,districtcode,street,address,age,SpecialtyType
                                                from EPG_APTITUDEINVESTIGATEPEOPLE p 
                                                    where p.id='{0}' ", item.ID, pwd, key, DateTime.Now.ToString("yyyy-MM-dd"));
                                res.ExecuteBySql(sql);

                                //ͨ����λID��ȡ��ɫ��Ϣ
                                sql = string.Format("select t.roleids,t.rolenames from BASE_ROLE t where t.organizeid='{0}' and t.roleid='{1}'", item.ORGANIZEID, item.DUTYID);
                                DataTable dtRoles = BaseRepository().FindTable(sql);

                                if (dtRoles != null && dtRoles.Rows.Count > 0)
                                {
                                    //���ݸ�λID������Ա��λ����
                                    string sqlRole = string.Format(@" update base_user b set b.rolename= '{0}',b.roleid='{2}' where b.dutyid = '{1}'", dtRoles.Rows[0]["rolenames"].ToString(), item.DUTYID, dtRoles.Rows[0]["roleids"].ToString());
                                    res.ExecuteBySql(sqlRole);
                                    string[] arr = dtRoles.Rows[0]["roleids"].ToString().Split(',');
                                    foreach (string roleId in arr)
                                    {
                                        //�����ɫ��Ա��ϵ��
                                        sql = string.Format(string.Format(@" insert into BASE_USERRELATION(USERRELATIONID,userid,Objectid,category,sortcode,isdefault) select '{1}' || rownum,p.id,'{2}','2',0,1
                                from EPG_APTITUDEINVESTIGATEPEOPLE p where p.id='{0}'", item.ID, Guid.NewGuid().ToString(), roleId));
                                        res.ExecuteBySql(sql);
                                    }
                                }

                                //ͬ����Ա֤����Ϣ
                                sql = string.Format(string.Format(@"insert into bis_certificate(id,userid,years,certname,certnum,senddate,sendorgan,filepath,certtype,WorkType,WorkItem,ItemNum,ApplyDate,StartDate,Grade,Industry,UserType,Craft,ZGName,EndDate)
                                                                    select c.id,c.userid,c.validttime,c.credentialsname,
                                                                    c.credentialscode,c.credentialstime,c.credentialsorg,
                                                                    c.credentialsfile,certtype,c.WorkType,c.WorkItem,c.ItemNum,c.ApplyDate,c.StartDate,c.Grade,c.Industry,c.UserType,c.Craft,c.ZGName,c.EndDate 
                                                                    from EPG_CERTIFICATEINSPECTORS c where c.userid in (select Id from EPG_APTITUDEINVESTIGATEPEOPLE p 
                                                    where p.id='{0}')", item.ID));
                                if (res.ExecuteBySql(sql) > 0)
                                {
                                    //ͬ��֤����Ƭ
                                    sql = string.Format(@"insert into BASE_FILEINFO(fileid,t.folderid,filename,filepath,filesize,t.fileextensions,t.filetype,recid) select fileid || rownum,t.folderid,filename,filepath,filesize,t.fileextensions,t.filetype,recid from BASE_FILEINFO t
where recid in(select id from EPG_CERTIFICATEINSPECTORS where userid in (select Id from EPG_APTITUDEINVESTIGATEPEOPLE p 
                                                    where p.id='{0}'))", item.ID);
                                    res.ExecuteBySql(sql);
                                }
                                userids.Add(item.ID);
                                //list.Add(userservice.GetEntity(item.ID));
                                #endregion
                            }
                            else
                            {
                                #region �޸�ͬ����Ա��֤��
                                string sql = "";
                                //ͬ����Ա��Ϣ
                                sql = string.Format(@"update base_user set (
                                                        encode,realname,headicon,birthday,mobile,telephone,email,oicq,wechat,
                                                        msn,organizeid,dutyid,dutyname,postid,postname,gender,isspecialequ,isspecial,
                                                        projectid,nation,native,usertype,isepiboly,degreesid,degrees,identifyid,
                                                        organizecode,createuserdeptcode,createuserorgcode,createdate,createuserid,
                                                        departmentid,departmentcode,ISPRESENCE,account,enabledmark,
                                                        craft,deletemark,entertime,isfourperson,fourpersontype,healthstatus,craftage,ACCOUNTTYPE,ISAPPLICATIONLDAP,quickquery,simplespelling,managerid,
                                                        manager,district,districtcode,street,address,age,SpecialtyType)=
                                               (select p.encode,p.realname,p.headicon,p.birthday,p.mobile,p.telephone,p.email,
                                                        p.oicq,p.wechat,p.msn,p.organizeid,p.dutyid,p.dutyname,p.postid,p.postname,
                                                        p.gender,p.isspecialequ,p.isspecial,p.OUTENGINEERID,p.nation,p.native,p.usertype,
                                                        p.isepiboly,p.degreesid,p.degrees,p.identifyid,p.organizecode,p.createuserdeptcode,
                                                        p.createuserorgcode,p.createdate,p.createuserid,p.outprojectid,p.outprojectcode,'1',
                                                        accounts as account,1,p.workoftype,0,to_date('{1}','yyyy-MM-dd'),
                                                        isfourperson,fourpersontype,stateofhealth,workyear,ACCOUNTTYPE,ISAPPLICATIONLDAP,quickquery,simplespelling,managerid,
                                                        manager,district,districtcode,street,address,age,SpecialtyType
                                                from EPG_APTITUDEINVESTIGATEPEOPLE p 
                                                    where p.id='{0}') where userid='{2}' ", item.ID, DateTime.Now.ToString("yyyy-MM-dd"), user.UserId);
                                res.ExecuteBySql(sql);
                                //��ѯ��λ��Ϣ
                                sql = string.Format("select dutyid,organizeid from EPG_APTITUDEINVESTIGATEPEOPLE p where p.id='{0}'", item.ID);
                                DataTable dt = BaseRepository().FindTable(sql);
                                foreach (DataRow dr in dt.Rows)
                                {
                                    //ͨ����λID��ȡ��ɫ��Ϣ
                                    sql = string.Format("select t.roleids,t.rolenames from BASE_ROLE t where t.organizeid='{0}' and t.roleid='{1}'", dr[1].ToString(), dr[0].ToString());
                                    DataTable dtRoles = BaseRepository().FindTable(sql);

                                    if (dtRoles != null && dtRoles.Rows.Count > 0)
                                    {
                                        //���ݸ�λID������Ա��λ����
                                        string sqlRole = string.Format(@" update base_user b set b.rolename= '{0}',b.roleid='{2}' where b.dutyid = '{1}'", dtRoles.Rows[0]["rolenames"].ToString(), dr["dutyid"].ToString(), dtRoles.Rows[0]["roleids"].ToString());
                                        res.ExecuteBySql(sqlRole);
                                        string sqlDelRelation = string.Format(@"delete BASE_USERRELATION where userid ='{0}'", user.UserId);
                                        res.ExecuteBySql(sqlDelRelation);
                                        string[] arr = dtRoles.Rows[0]["roleids"].ToString().Split(',');
                                        foreach (string roleId in arr)
                                        {
                                            //�����ɫ��Ա��ϵ��
                                            sql = string.Format(string.Format(@" insert into BASE_USERRELATION(USERRELATIONID,userid,Objectid,category,sortcode,isdefault) select '{1}' || rownum,'{3}','{2}','2',0 ,1
                                from EPG_APTITUDEINVESTIGATEPEOPLE p where p.id='{0}'", item.ID, Guid.NewGuid().ToString(), roleId, user.UserId));
                                            res.ExecuteBySql(sql);
                                        }
                                    }
                                }
                                //ͬ����Ա֤����Ϣ
                                sql = string.Format(string.Format(@"insert into bis_certificate(id,userid,years,certname,certnum,senddate,sendorgan,filepath,certtype,WorkType,WorkItem,ItemNum,ApplyDate,StartDate,Grade,Industry,UserType,Craft,ZGName,EndDate)
                                                                    select c.id,'{1}',c.validttime,c.credentialsname,
                                                                    c.credentialscode,c.credentialstime,c.credentialsorg,
                                                                    c.credentialsfile,certtype,c.WorkType,c.WorkItem,c.ItemNum,c.ApplyDate,c.StartDate,c.Grade,c.Industry,c.UserType,c.Craft,c.ZGName,c.EndDate 
                                                                    from EPG_CERTIFICATEINSPECTORS c where c.userid ='{0}'", item.ID, user.UserId));
                                if (res.ExecuteBySql(sql) > 0)
                                {
                                    //ͬ��֤����Ƭ
                                    sql = string.Format(@"insert into BASE_FILEINFO(fileid,t.folderid,filename,filepath,filesize,t.fileextensions,t.filetype,recid) select fileid || rownum,t.folderid,filename,filepath,filesize,t.fileextensions,t.filetype,'{1}' from BASE_FILEINFO t
where recid in(select id from EPG_CERTIFICATEINSPECTORS where userid ='{0}')", item.ID, user.UserId);
                                    res.ExecuteBySql(sql);
                                }
                                userids.Add(user.UserId);
                                //list.Add(userservice.GetEntity(user.UserId));
                                #endregion
                            }

                        }
                        
                    }
                    else
                    {
                        p_entity.NEXTAUDITROLE = nextCheck.CHECKROLENAME;
                        p_entity.NEXTAUDITDEPTID = nextCheck.CHECKDEPTID;
                        p_entity.NEXTAUDITDEPTCODE = nextCheck.CHECKDEPTCODE;
                        p_entity.FlowId = nextCheck.ID;
                        p_entity.ISAUDITOVER = "0";
                    }
                    res.Update<PeopleReviewEntity>(p_entity);
                }
                else
                {
                    p_entity.ISSAVEORCOMMIT = "0";
                    p_entity.NEXTAUDITDEPTCODE = "";
                    p_entity.NEXTAUDITDEPTID = "";
                    p_entity.NEXTAUDITROLE = "";
                    p_entity.FlowId = "";
                    res.Update<PeopleReviewEntity>(p_entity);
                    //��˲�ͨ�������ʷ��¼
                    var str = JsonConvert.SerializeObject(p_entity);
                    HisPeopleReviewEntity hrd = JsonConvert.DeserializeObject<HisPeopleReviewEntity>(str);
                    hrd.ID = Guid.NewGuid().ToString();
                    hrd.HISPEOPLEREVIEWID = p_entity.ID;
                    res.Insert<HisPeopleReviewEntity>(hrd);
                    #region ��Ա��¼--������ʷ��¼
                    Repository<AptitudeinvestigatepeopleEntity> repPeople = new Repository<AptitudeinvestigatepeopleEntity>(DbFactory.Base());
                    List<AptitudeinvestigatepeopleEntity> list_people = repPeople.FindList(string.Format(@"select * from epg_aptitudeinvestigatepeople t where t.outengineerid='{0}' and t.outprojectid='{1}' and t.peoplereviewid='{2}'", p_entity.OUTENGINEERID, outeng.OUTPROJECTID, p_entity.ID)).ToList();
                    List<HistoryPeople> his_people = new List<HistoryPeople>();
                    if (list_people.Count > 0)
                    {
                        var str_people = JsonConvert.SerializeObject(list_people);
                        his_people = JsonConvert.DeserializeObject<List<HistoryPeople>>(str_people);
                        for (int i = 0; i < his_people.Count; i++)
                        {
                            his_people[i].ID = Guid.NewGuid().ToString();
                            his_people[i].HISPEOPLEREVIEWID = hrd.ID;
                            his_people[i].USERID = list_people[i].ID;
                            var key1 = list_people[i].ID + "01";
                            var key2 = list_people[i].ID + "02";
                            var key3 = list_people[i].ID + "03";
                            var key4 = list_people[i].ID + "04";
                            //ͬ������
                            var file1 = fileService.GetFiles(key1);
                            var file2 = fileService.GetFiles(key2);
                            var file3 = fileService.GetFiles(key3);
                            var file4 = fileService.GetFiles(key4);

                            if (file1.Rows.Count > 0)
                            {
                                var key = his_people[i].ID + "01";
                                foreach (DataRow item in file1.Rows)
                                {
                                    FileInfoEntity itemFile = new FileInfoEntity();
                                    itemFile.FileName = item["FileName"].ToString();
                                    itemFile.FilePath = item["filepath"].ToString();
                                    itemFile.FileSize = item["filesize"].ToString();
                                    itemFile.RecId = key;
                                    fileService.SaveForm(itemFile.FileId, itemFile);
                                }
                            }
                            if (file2.Rows.Count > 0)
                            {
                                var key = his_people[i].ID + "02";
                                foreach (DataRow item in file2.Rows)
                                {
                                    FileInfoEntity itemFile = new FileInfoEntity();
                                    itemFile.FileName = item["FileName"].ToString();
                                    itemFile.FilePath = item["filepath"].ToString();
                                    itemFile.FileSize = item["filesize"].ToString();
                                    itemFile.RecId = key;
                                    fileService.SaveForm(itemFile.FileId, itemFile);
                                }
                            }
                            if (file3.Rows.Count > 0)
                            {
                                var key = his_people[i].ID + "03";
                                foreach (DataRow item in file3.Rows)
                                {
                                    FileInfoEntity itemFile = new FileInfoEntity();
                                    itemFile.FileName = item["FileName"].ToString();
                                    itemFile.FilePath = item["filepath"].ToString();
                                    itemFile.FileSize = item["filesize"].ToString();
                                    itemFile.RecId = key;
                                    fileService.SaveForm(itemFile.FileId, itemFile);
                                }
                            }
                            if (file4.Rows.Count > 0)
                            {
                                var key = his_people[i].ID + "04";
                                foreach (DataRow item in file4.Rows)
                                {
                                    FileInfoEntity itemFile = new FileInfoEntity();
                                    itemFile.FileName = item["FileName"].ToString();
                                    itemFile.FilePath = item["filepath"].ToString();
                                    itemFile.FileSize = item["filesize"].ToString();
                                    itemFile.RecId = key;
                                    fileService.SaveForm(itemFile.FileId, itemFile);
                                }
                            }
                        }
                        res.Insert<HistoryPeople>(his_people);
                    }
                    #region ֤����¼--������ʷ��¼
                    foreach (HistoryPeople item in his_people)
                    {

                        Repository<CertificateinspectorsEntity> certificate = new Repository<CertificateinspectorsEntity>(DbFactory.Base());
                        List<CertificateinspectorsEntity> cerList = certificate.FindList(string.Format("select * from epg_certificateinspectors t where t.userid='{0}'", item.USERID)).ToList();
                        if (cerList.Count > 0)
                        {
                            var cer_List = JsonConvert.SerializeObject(cerList);
                            List<HistoryCertificate> his_Certificate = JsonConvert.DeserializeObject<List<HistoryCertificate>>(cer_List);
                            StringBuilder sb = new StringBuilder("begin\r\n");
                            for (int i = 0; i < his_Certificate.Count; i++)
                            {
                                string newId= Guid.NewGuid().ToString();
                              
                                his_Certificate[i].HISUSERID = item.ID;
                                sb.AppendFormat("insert into BASE_FILEINFO(fileid,folderid,filename,filepath,filesize,fileextensions,filetype,recid,DeleteMark) select '{2}' || rownum,folderid,filename,filepath,filesize,fileextensions,filetype,'{1}',0 from BASE_FILEINFO t where recid='{0}';\r\n ", his_Certificate[i].ID, newId, Guid.NewGuid().ToString());
                                his_Certificate[i].ID = newId;
                            }
                            sb.Append("end;");
                            res.Insert<HistoryCertificate>(his_Certificate);
                            res.ExecuteBySql(sb.ToString());
                        }
                    }
                    #endregion
                    #endregion
                    #region ��˼�¼--������ʷ��¼
                    entity.APTITUDEID = hrd.ID;
                    Repository<AptitudeinvestigateauditEntity> audit = new Repository<AptitudeinvestigateauditEntity>(DbFactory.Base());
                    List<AptitudeinvestigateauditEntity> list = audit.FindList(string.Format(@"select * from  EPG_APTITUDEINVESTIGATEAUDIT t where t.APTITUDEID='{0}'", p_entity.ID)).ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].APTITUDEID = hrd.ID;
                    }
                    res.Update<AptitudeinvestigateauditEntity>(list);
                    #endregion
                }
                entity.Create();
                res.Insert<AptitudeinvestigateauditEntity>(entity);
                res.Commit();
                if (p_entity.ISAUDITOVER == "1")
                {
                    //ͬ����Ա������
                    if (!string.IsNullOrWhiteSpace(new DataItemDetailService().GetItemValue("bzAppUrl")))
                    {
                        foreach (var item in userids)
                        {
                            var temp = userservice.GetEntity(item);
                            if (temp!=null)
                            {
                                UserEntitylist.Add(temp);
                            }
                        }
                        peopleReview.SaveUser(UserEntitylist);
                    }
                    //ͬ�������Ա���Ƽ�MISϵͳ(��������)
                    var isGdxy = new ERCHTMS.Service.SystemManage.DataItemDetailService().GetItemValue("IsGdxy");
                    if (!string.IsNullOrWhiteSpace(isGdxy))
                    {
                        new DepartmentService().SyncUsers(peoplelist);

                    }
                    return userids;
                }
               
            }
            catch (Exception ex)
            {
                res.Rollback();
            }
            return new List<string>();
        }
        public string GetItemValue(string itemName)
        {
            string sql = string.Format("select t.itemvalue from base_dataitemdetail t where itemname='{0}'", itemName);
            object obj = new RepositoryFactory().BaseRepository().FindObject(sql);
            return obj == null || obj == System.DBNull.Value ? "" : obj.ToString();
        }
        #endregion
    }
}
