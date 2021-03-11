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
    /// �� ������ȫ���չܿ�ȡֵ���ñ�
    /// </summary>
    public class RiskwayconfigService : RepositoryFactory<RiskwayconfigEntity>, RiskwayconfigIService
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
            if (!queryParam["WayType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and WayTypeCode ='{0}'", queryParam["WayType"].ToString());
            }

            if (!queryParam["RiskType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and RiskTypeCode ='{0}'", queryParam["RiskType"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<RiskwayconfigEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public RiskwayconfigEntity GetEntity(string keyValue)
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
            var res = DbFactory.Base().BeginTrans();
            try
            {
                string sql = string.Format("delete from BIS_RISKWAYCONFIGDETAIL t where t.WAYCONFIGID='{0}'", keyValue);
                res.ExecuteBySql(sql);
                res.Delete<RiskwayconfigEntity>(keyValue);
                res.Commit();
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
        public void SaveForm(string keyValue, RiskwayconfigEntity entity, List<RiskwayconfigdetailEntity> list)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    res.Update<RiskwayconfigEntity>(entity);
                }
                else
                {
                    entity.Create();
                    res.Insert<RiskwayconfigEntity>(entity);
                }
                string sql = string.Format("delete from BIS_RISKWAYCONFIGDETAIL t where t.WAYCONFIGID='{0}'", entity.ID);
                res.ExecuteBySql(sql);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Create();
                    list[i].WayConfigId = entity.ID;
                }
                res.Insert<RiskwayconfigdetailEntity>(list);

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
