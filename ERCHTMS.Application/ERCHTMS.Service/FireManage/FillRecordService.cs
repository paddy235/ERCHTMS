using ERCHTMS.Entity.FireManage;
using ERCHTMS.IService.FireManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;

namespace ERCHTMS.Service.FireManage
{
    /// <summary>
    /// �� ������װ/������¼
    /// </summary>
    public class FillRecordService : RepositoryFactory<FillRecordEntity>, FillRecordIService
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
        public IEnumerable<FillRecordEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from HRS_FILLRECORD where 1=1 " + queryJson).ToList();
            //return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public FillRecordEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, FillRecordEntity entity)
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
                try
                {
                    //����������ʱ�䡢�´μ��ʱ��
                    IRepository db = new RepositoryFactory().BaseRepository();
                    FirefightingEntity fe = db.FindEntity<FirefightingEntity>(entity.EquipmentId);
                    if (fe != null)
                    {
                        fe.LastFillDate = entity.FillDate;
                        if (!string.IsNullOrEmpty(fe.FillPeriod.Value.ToString()))
                        {
                            fe.NextFillDate = entity.FillDate.Value.AddDays(fe.FillPeriod.Value);
                        }
                        db.Update<FirefightingEntity>(fe);

                    }
                }
                catch { }
            }
        }
        #endregion
    }
}
