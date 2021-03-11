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
    /// �� ����������ʩ-���ڼ���¼
    /// </summary>
    public class TerminalDetectionRecordService : RepositoryFactory<TerminalDetectionRecordEntity>, TerminalDetectionRecordIService
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
                    pagination.conditionJson += string.Format(" and t.EquipmentId='{0}'", queryParam["EquipmentId"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TerminalDetectionRecordEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from HRS_TERMINALDETECTIONRECORD where 1=1 " + queryJson).ToList();
            //return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TerminalDetectionRecordEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, TerminalDetectionRecordEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                TerminalDetectionRecordEntity te = this.BaseRepository().FindEntity(keyValue);
                if (te == null)
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
        public void SaveFirefightingEntity(TerminalDetectionRecordEntity entity)
        {

            try
            {
                //���������ڼ��ʱ�䡢�´ζ��ڼ��ʱ��
                IRepository db = new RepositoryFactory().BaseRepository();
                FirefightingEntity fe = db.FindEntity<FirefightingEntity>(entity.EquipmentId);
                if (fe != null)
                {
                    fe.TerminalDetectionDate = entity.DetectionDate;
                    if (!string.IsNullOrEmpty(fe.TerminalDetectionPeriod.Value.ToString()))
                    {
                        fe.NextTerminalDetectionDate = entity.DetectionDate.Value.AddDays(fe.TerminalDetectionPeriod.Value);
                    }
                    fe.TerminalDetectionUnit = entity.DetectionUnit;
                    fe.TerminalDetectionVerdict = entity.DetectionResult;
                    db.Update<FirefightingEntity>(fe);
                }
            }
            catch { }
        }
        #endregion
    }
}
