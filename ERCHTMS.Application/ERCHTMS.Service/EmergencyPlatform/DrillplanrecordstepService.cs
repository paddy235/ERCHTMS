using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util.Extension;
using BSFramework.Util;

namespace ERCHTMS.Service.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ��������¼�����
    /// </summary>
    public class DrillplanrecordstepService : RepositoryFactory<DrillplanrecordstepEntity>, DrillplanrecordstepIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<DrillplanrecordstepEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public DrillplanrecordstepEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// ����recid��ȡ�����б�
        /// </summary>
        /// <param name="recid"></param>
        /// <returns></returns>
        public IList<DrillplanrecordstepEntity> GetListByRecid(string recid)
        {
            var list = this.BaseRepository().IQueryable().Where(x => x.DrillStepRecordId == recid).OrderBy(t => t.SortId).ToList();
            return list;
        }

        /// <summary>
        /// Ӧ����¼�����б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            if (!queryJson.IsEmpty())
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["recid"].IsEmpty())
                {
                    pagination.conditionJson += " and drillsteprecordid = '" + queryParam["recid"].ToString() + "'";
                }
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        #endregion

        #region �ύ����

        /// <summary>
        /// ���ݹ���IDɾ������
        /// </summary>
        /// <param name="recid"></param>
        public void RemoveFormByRecid(string recid)
        {
            string sql = "delete MAE_DRILLPLANRECORDSTEP where DRILLSTEPRECORDID='" + recid + "'";
            this.BaseRepository().ExecuteBySql(sql);
        }

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
        public void SaveForm(string keyValue, DrillplanrecordstepEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                DrillplanrecordstepEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se == null)
                {
                    entity.Id = keyValue;
                    entity.Create();
                    this.BaseRepository().Insert(entity);


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
            }
        }
        #endregion
    }
}
