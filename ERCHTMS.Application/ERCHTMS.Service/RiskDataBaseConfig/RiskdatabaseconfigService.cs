using ERCHTMS.Entity.RiskDataBaseConfig;
using ERCHTMS.IService.RiskDataBaseConfig;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.RiskDataBaseConfig
{
    /// <summary>
    /// �� ������ȫ���չܿ����ñ�
    /// </summary>
    public class RiskdatabaseconfigService : RepositoryFactory<RiskdatabaseconfigEntity>, RiskdatabaseconfigIService
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
            var queryParam = queryJson.ToJObject();
            if (!queryParam["DataType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and DataType ='{0}'", queryParam["DataType"].ToString());
            }
            if (!queryParam["RiskType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and RiskTypeCode ='{0}'", queryParam["RiskType"].ToString());
            }
            if (!queryParam["ConfigType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and ConfigTypeCode ='{0}'", queryParam["ConfigType"].ToString());
            }
            if (!queryParam["ItemType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and ItemTypeCode ='{0}'", queryParam["ItemType"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ����SQL����ȡ���ݼ�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetTable(string sql)
        {
            return this.BaseRepository().FindTable(sql);
        }
      
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<RiskdatabaseconfigEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public RiskdatabaseconfigEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, RiskdatabaseconfigEntity entity)
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
