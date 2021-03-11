using ERCHTMS.Entity.SafetyWorkSupervise;
using ERCHTMS.IService.SafetyWorkSupervise;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;

namespace ERCHTMS.Service.SafetyWorkSupervise
{
    /// <summary>
    /// �� ������ȫ�ص㹤�����췴����Ϣ
    /// </summary>
    public class SafetyworkfeedbackService : RepositoryFactory<SafetyworkfeedbackEntity>, SafetyworkfeedbackIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafetyworkfeedbackEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            pagination.conditionJson = string.Format(@" t.superviseid='{0}' and t1.id is not null and superviseresult='1' ", queryJson);
            #region ���
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.superviseid,t1.id as cid,to_char(t.feedbackdate,'yyyy-MM-dd') as feedbackdate,t.finishinfo,t.signurl,t1.superviseresult,t1.superviseopinion,t1.signurl as signurlt,to_char(t1.confirmationdate,'yyyy-MM-dd') as confirmationdate";
            pagination.p_tablename = @"BIS_SafetyWorkFeedback t left join BIS_SuperviseConfirmation t1 on t.id=t1.feedbackid";
            if (pagination.sidx == null)
            {
                pagination.sidx = "t1.createdate";
            }
            if (pagination.sord == null)
            {
                pagination.sord = "desc";
            }
            #endregion

            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return data;
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SafetyworkfeedbackEntity GetEntity(string keyValue)
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
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafetyworkfeedbackEntity entity)
        {
            bool b = false;
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                var sl = BaseRepository().FindEntity(keyValue);
                if (sl != null)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    b = true;
                }
            }
            else
            {
                b = true;
            }
            if (b)
            {
                entity.Id = keyValue;
                entity.Create();
                this.BaseRepository().Insert(entity);
                Repository<SafetyworksuperviseEntity> announRes = new Repository<SafetyworksuperviseEntity>(DbFactory.Base());
                var sl = announRes.FindEntity(entity.SuperviseId);
                if (sl != null)
                {   //������������״̬
                    sl.FlowState = "2";
                    announRes.Update(sl);
                }
            }
        }
        #endregion
    }
}
