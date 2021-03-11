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
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// �� ������վ�ල����
    /// </summary>
    public class SuperviseTaskService : RepositoryFactory<SuperviseTaskEntity>, SuperviseTaskIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<SuperviseTaskEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SuperviseTaskEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(string.Format("select * from bis_supervisetask where 1=1 " + queryJson)).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SuperviseTaskEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// ��ȡ�ල�����б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            var user = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["status"].IsEmpty())//�ල״̬
            {
                pagination.conditionJson += string.Format(" and supervisestate='{0}'", queryParam["status"].ToString());
            }
            if (!queryParam["worktype"].IsEmpty())//��ҵ����
            {
                pagination.conditionJson += string.Format(" and  ','||taskworktypeid ||',' like '%,{0},%'", queryParam["worktype"].ToString());
            }
            //ʱ��ѡ��
            if (!queryParam["st"].IsEmpty())//��ҵ��ʼʱ��
            {
                string from = queryParam["st"].ToString().Trim();
                pagination.conditionJson += string.Format(" and taskWorkStartTime>=to_date('{0}','yyyy-mm-dd')", from);
            }
            if (!queryParam["et"].IsEmpty())//��ҵ����ʱ��
            {
                string to = Convert.ToDateTime(queryParam["et"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and taskWorkEndTime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            if (!queryParam["workdept"].IsEmpty() && !queryParam["workdeptid"].IsEmpty())//��ҵ��λ
            {
                pagination.conditionJson += string.Format(" and id in(select t.superviseid  from bis_superviseworkinfo t where  workdeptcode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}') group by superviseid)", queryParam["workdept"].ToString(), queryParam["workdeptid"].ToString());
            }
            if (!queryParam["sideuser"].IsEmpty())//�ලԱ
            {
                pagination.conditionJson += string.Format(" and TaskUserId  like '%{0}%'", queryParam["sideuser"].ToString());
            }
            if (!queryParam["teams"].IsEmpty())//����
            {
                pagination.conditionJson += string.Format(" and steamid='{0}'", queryParam["teams"].ToString());
            }
            if (!queryParam["parentid"].IsEmpty())//��������
            {
                pagination.conditionJson += string.Format(" and superparentid='{0}'", queryParam["parentid"].ToString());
            }
            else//Ĭ��ֻ��ʾһ������
            {
                if (!queryParam["mytask"].IsEmpty())//�ҵļල����
                {
                    pagination.conditionJson += string.Format(" and supervisestate!=1 and TaskUserId  like '%{0}%' and  IsSubmit='0' and tasklevel='1'",user.UserId);
                }
                else
                {
                    pagination.conditionJson += " and tasklevel='0'";
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
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
        /// <param name="model">ʵ�����</param>
        /// <returns></returns>
        public string SaveForm(string keyValue, SuperviseTaskModel model)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {

                Repository<SuperviseTaskEntity> repScaffold = new Repository<SuperviseTaskEntity>(DbFactory.Base());
                SuperviseTaskEntity entity = repScaffold.FindEntity(keyValue);
                model.Id = keyValue;
                //����
                if (entity == null)
                {
                    entity = new SuperviseTaskEntity();
                    entity.Id = Guid.NewGuid().ToString();
                    keyValue = entity.Id;
                    //ʵ�帳ֵ
                    this.copyProperties(entity, model);
                    entity.Create();
                    //��Ӳ���
                    res.Insert(entity);
                }
                else
                {
                    //�༭ 
                    entity.Modify(keyValue);
                    //ʵ�帳ֵ
                    this.copyProperties(entity, model);
                    //���²���
                    res.Update(entity);
                }
                //��ӻ������ҵ��Ϣ ��ɾ�������
                res.Delete<SuperviseWorkInfoEntity>(t => t.SuperviseId == entity.Id);
                foreach (var spec in model.WorkSpecs)
                {
                    spec.SuperviseId = entity.Id;
                    spec.Create();
                    res.Insert(spec);
                }
                res.Commit();
                return keyValue;
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }


        /// <summary>
        /// ��Դʵ���Ŀ��ʵ�����Ը�ֵ
        /// </summary>
        /// <param name="target">Ŀ��</param>
        /// <param name="source">Դ</param>
        private void copyProperties(SuperviseTaskEntity target, SuperviseTaskModel source)
        {
            target.TaskLevel = source.TaskLevel;
            target.IsSubmit = "0";//һ��ʼδ�ύ
            target.TimeLong = source.TimeLong;
            target.OrganizeManager = source.OrganizeManager;
            target.SuperviseCode = source.SuperviseCode;
            target.SuperviseState = source.SuperviseState;
            target.TaskWorkEndTime = source.TaskWorkEndTime;
            target.TimeLongStr = source.TimeLongStr;
            target.TaskWorkTypeId = source.TaskWorkTypeId;
            target.TaskUserName = source.TaskUserName;
            target.TaskWorkType = source.TaskWorkType;
            target.TaskWorkStartTime = source.TaskWorkStartTime;
            target.TaskUserId = source.TaskUserId;
            target.RiskAnalyse = source.RiskAnalyse;
            target.SafetyMeasure = source.SafetyMeasure;
            target.TaskBill = source.TaskBill;
            target.RiskAnalyse = source.RiskAnalyse;
            target.SuperParentId = source.SuperParentId;
            target.ConstructLayout = source.ConstructLayout;
            target.STeamId = source.STeamId;
            target.STeamCode = source.STeamCode;
            target.STeamName = source.STeamName;
            target.HandType = source.HandType;
            target.CreateUserId = source.CreateUserId;
            target.CreateUserName = source.CreateUserName;
            target.CreateUserDeptCode = source.CreateUserDeptCode;
            target.CreateUserOrgCode = source.CreateUserOrgCode;
        }


        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveOnlyTask(string keyValue, SuperviseTaskEntity entity)
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
