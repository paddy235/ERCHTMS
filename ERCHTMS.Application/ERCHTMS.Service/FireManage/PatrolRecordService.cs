using ERCHTMS.Entity.FireManage;
using ERCHTMS.IService.FireManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.FireManage
{
    /// <summary>
    /// �� ����Ѳ���¼���ص����λ�ӱ�
    /// </summary>
    public class PatrolRecordService : RepositoryFactory<PatrolRecordEntity>, PatrolRecordIService
    {
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
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                //��ѯ����
                if (!queryParam["MainId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and MainId='{0}'", queryParam["MainId"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<PatrolRecordEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from HRS_PATROLRECORD where 1=1 " + queryJson).ToList();
            //return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public PatrolRecordEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, PatrolRecordEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                PatrolRecordEntity pe = this.BaseRepository().FindEntity(keyValue);
                if (pe == null)
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                    SavePartEntity(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
                SavePartEntity(entity);
            }
        }
        public void SavePartEntity(PatrolRecordEntity entity) {
            try
            {
                //���������´�Ѳ��ʱ�䡢���Ѳ��ʱ��
                IRepository db = new RepositoryFactory().BaseRepository();
                KeyPartEntity ke = db.FindEntity<KeyPartEntity>(entity.MainId);
                if (ke != null)
                {

                    ke.LatelyPatrolDate = entity.PatrolDate;
                    if (entity.PatrolPeriod != null)
                    {
                        ke.NextPatrolDate = entity.PatrolDate.Value.AddDays(entity.PatrolPeriod.Value);
                    }
                    else
                    {
                        ke.NextPatrolDate = entity.NextPatrolDate;
                    }
                    db.Update<KeyPartEntity>(ke);
                }
            }
            catch { }
        }
        #endregion
    }
}
