using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;

namespace ERCHTMS.Service.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ�����ʿ��
    /// </summary>
    public class SuppliesfactoryService : RepositoryFactory<SuppliesfactoryEntity>, SuppliesfactoryIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SuppliesfactoryEntity> GetList(string queryJson)
        {
            Operator user = OperatorProvider.Provider.Current();
            return this.BaseRepository().IQueryable().Where(t => t.CreateUserOrgCode == user.OrganizeCode).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SuppliesfactoryEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// Ӧ����Դ���б�
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
                if (!queryParam["name"].IsEmpty())
                {
                    pagination.conditionJson += " and name like '%" + queryParam["name"].ToString() + "%'";
                }
                if (!queryParam["suppliestype"].IsEmpty())
                {
                    pagination.conditionJson += " and suppliestype ='" + queryParam["suppliestype"].ToString() + "'";
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
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SuppliesfactoryEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                SuppliesfactoryEntity se = this.BaseRepository().FindEntity(keyValue);
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
