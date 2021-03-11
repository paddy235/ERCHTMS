using ERCHTMS.Entity.FireManage;
using ERCHTMS.IService.FireManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.FireManage
{
    /// <summary>
    /// �� ������⡢ά����¼
    /// </summary>
    public class DetectionRecordService : RepositoryFactory<DetectionRecordEntity>, DetectionRecordIService
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
                if (!queryParam["EquipmentId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and EquipmentId='{0}'", queryParam["EquipmentId"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<DetectionRecordEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from HRS_DETECTIONRECORD where 1=1 " + queryJson).ToList();
            //return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public DetectionRecordEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, DetectionRecordEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                DetectionRecordEntity de = this.BaseRepository().FindEntity(keyValue);
                if (de == null)
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                    SaveFirefightingEntity(entity);
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
                SaveFirefightingEntity(entity);
            }
        }
        public void SaveFirefightingEntity(DetectionRecordEntity entity) {

            try
            {
                //��������ά��ʱ�䡢�´�ά��ʱ��
                IRepository db = new RepositoryFactory().BaseRepository();
                FirefightingEntity fe = db.FindEntity<FirefightingEntity>(entity.EquipmentId);
                if (fe != null)
                {
                    fe.DetectionDate = entity.DetectionDate;
                    if (!string.IsNullOrEmpty(fe.DetectionPeriod.Value.ToString()))
                    {
                        fe.NextDetectionDate = entity.DetectionDate.Value.AddDays(fe.DetectionPeriod.Value);
                    }
                    fe.DetectionVerdict = entity.Conclusion;
                    db.Update<FirefightingEntity>(fe);
                }
            }
            catch { }
        }
        #endregion
    }
}
