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
using ERCHTMS.Code;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// �� ����������ʩ����
    /// </summary>
    public class SchemeMeasureService : RepositoryFactory<SchemeMeasureEntity>, SchemeMeasureIService
    {
        private OutsouringengineerService outsouringengineerservice = new OutsouringengineerService();
        private DepartmentService departmentservice = new DepartmentService();
        private ManyPowerCheckService powerCheck = new ManyPowerCheckService();
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!string.IsNullOrEmpty(queryJson))
            {
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
                    pagination.conditionJson += string.Format(" and to_date(to_char(t.SubmitDate,'yyyy-MM-dd'),'yyyy-MM-dd') between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
                }
                //��ѯ����
                if (!queryParam["condition"].IsEmpty() && !queryParam["txtSearch"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and {0} like '%{1}%'", queryParam["condition"].ToString(), queryParam["txtSearch"].ToString());
                }
                if (!queryParam["BELONGMAJOR"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and t.BELONGMAJOR ='{0}' ", queryParam["BELONGMAJOR"].ToString());
                }
                if (!queryParam["outengineerid"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and t.projectid ='{0}' ", queryParam["outengineerid"].ToString());
                }
                if (!queryParam["orgCode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and t.createuserorgcode ='{0}' ", queryParam["orgCode"].ToString());
                }
                if (!queryParam["indexState"].IsEmpty())//��ҳ����
                {
                    string strCondition = "";
                    strCondition = string.Format(" and t.createuserorgcode='{0}' and t.isover='0' and t.issaved='1'", user.OrganizeCode);
                    DataTable data = BaseRepository().FindTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson + strCondition);
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        var engineerEntity = outsouringengineerservice.GetEntity(data.Rows[i]["engineerid"].ToString());
                        if (engineerEntity != null)
                        {
                            var excutdept = departmentservice.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                            var outengineerdept = departmentservice.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                            var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentservice.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                            //��ȡ��һ�������
                            string str = powerCheck.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, data.Rows[i]["engineerid"].ToString());
                            data.Rows[i]["approveuserids"] = str;
                        }
                        else
                        {
                            string str = powerCheck.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), "", "", data.Rows[i]["kbengineerletdeptid"].ToString(), "", "", "", "", "", data.Rows[i]["engineerid"].ToString());
                            data.Rows[i]["approveuserids"] = str;
                        }

                    }

                    string[] applyids = data.Select(" approveuserids like '%" + user.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();

                    pagination.conditionJson += string.Format(" and t.id in ('{0}') {1}", string.Join("','", applyids), strCondition);
                }
                if (!queryParam["projectid"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and r.id='{0}'", queryParam["projectid"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SchemeMeasureEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        public IEnumerable<SchemeMeasureEntity> GetList()
        {
            return this.BaseRepository().IQueryable();
        }

        /// <summary>
        /// ��ȡ������������������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetObjectByKeyValue(string keyValue) 
        {
            string sql = string.Format(@" select  r.id as engineerid,e.fullname ,r.engineername,r.engineercode, r.engineerletdept, 
                                            to_char(t.submitdate,'yyyy-mm-dd') as submitdate,t.submitperson,t.summitcontent,
                                            r.engineerdirector,r.createdate engineerdate,t.createuserid ,t.issaved,t.isover,t.flowdeptname,
                                            t.flowdept,t.flowrolename,t.flowrole,t.flowname,t.organizer,t.organiztime,r.supervisorname
                                    from  epg_schememeasure t 
                                    left join epg_outsouringengineer r  on t.projectid=r.id 
                                    left join base_department e on r.outprojectid=e.departmentid where t.id='{0}'", keyValue );

            DataTable dt = this.BaseRepository().FindTable(sql);

            return dt;
        }


        /// <summary>
        /// ��ȡ���һ�����ͨ��������������Ϣ
        /// </summary>
        /// <param name="keyValue">ProjectID</param>
        /// <returns></returns>
        public SchemeMeasureEntity GetSchemeMeasureListByOutengineerId(string keyValue)
        {
            string sql = string.Format(@" select t.ID,  r.id as engineerid,e.fullname ,r.engineername,r.engineercode, r.engineerletdept, to_char(t.submitdate,'yyyy-mm-dd') as submitdate,t.submitperson,
                                    r.engineerdirector,r.createdate engineerdate,t.createuserid ,t.issaved,t.isover,t.flowdeptname,t.flowdept,t.flowrolename,t.flowrole,t.flowname,t.organizer,t.organiztime from  epg_schememeasure t 
                                    left join epg_outsouringengineer r  on t.projectid=r.id 
                                    left join base_department e on r.outprojectid=e.departmentid where t.projectid='{0}' order by t.CREATEDATE desc", keyValue);
            return this.BaseRepository().FindList(sql).ToList().FirstOrDefault();
        }


        /// <summary>
        /// ��ȡ��ʷ������������������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetHistoryObjectByKeyValue(string keyValue) 
        {
            string sql = string.Format(@" select  r.id as engineerid,e.fullname ,r.engineername,r.engineercode, r.engineerletdept, to_char(t.submitdate,'yyyy-mm-dd') as submitdate,t.submitperson,t.summitcontent,
                                    r.engineerdirector,r.createdate engineerdate,t.createuserid ,t.issaved,t.isover,t.flowdeptname,t.flowdept,t.flowrolename,t.flowrole,t.flowname,t.organizer,t.organiztime from  epg_historyschememeasure t 
                                    left join epg_outsouringengineer r  on t.projectid=r.id 
                                    left join base_department e on r.outprojectid=e.departmentid where t.id='{0}'", keyValue);

            DataTable dt = this.BaseRepository().FindTable(sql);

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
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                SchemeMeasureEntity se = this.BaseRepository().FindEntity(keyValue);
                this.BaseRepository().Delete(keyValue);
                string sql = string.Format(@"select t.id from EPG_SchemeMeasure t where t.ProjectID='{0}'", se.PROJECTID);
                DataTable dt = this.BaseRepository().FindTable(sql);
                if (dt == null || dt.Rows.Count <= 0)
                {
                    #region ���¹�������״̬
                    Repository<StartappprocessstatusEntity> proecss = new Repository<StartappprocessstatusEntity>(DbFactory.Base());
                    StartappprocessstatusEntity startProecss = proecss.FindList(string.Format("select * from epg_startappprocessstatus t where t.outengineerid='{0}'", se.PROJECTID)).ToList().FirstOrDefault();
                    startProecss.THREETWOSTATUS = "0";
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
        public void SaveForm(string keyValue, SchemeMeasureEntity entity)
        {
            entity.ID = keyValue;
            //��ʼ����
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    SchemeMeasureEntity se = this.BaseRepository().FindEntity(keyValue);
                    if (se == null)
                    {
                        entity.Create();
                        res.Insert<SchemeMeasureEntity>(entity);

                   
                    }
                    else
                    {
                        entity.Modify(keyValue);
                        res.Update<SchemeMeasureEntity>(entity);
                    }
                }
                else
                {
                    entity.Create();
                    res.Insert<SchemeMeasureEntity>(entity);
                }

                #region ���¹�������״̬
                //���״̬�¸���
                if (entity.ISOVER == "1" && entity.PROJECTID != "" && entity.PROJECTID != null)
                {
                    Repository<StartappprocessstatusEntity> proecss = new Repository<StartappprocessstatusEntity>(DbFactory.Base());
                    StartappprocessstatusEntity startProecss = proecss.FindList(string.Format("select * from epg_startappprocessstatus t where t.outengineerid='{0}'", entity.PROJECTID)).ToList().FirstOrDefault();
                    startProecss.THREETWOSTATUS = "1";
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
        #endregion
    }
}
