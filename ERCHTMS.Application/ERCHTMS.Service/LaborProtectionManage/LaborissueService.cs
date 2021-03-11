using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.IService.LaborProtectionManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ��������ż�¼��
    /// </summary>
    public class LaborissueService : RepositoryFactory<LaborissueEntity>, LaborissueIService
    {
        #region ��ȡ����

        /// <summary>
        /// �洢���̷�ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageListByProc(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //��ѯ����
            if (!queryParam["time"].IsEmpty())
            {
                string name = queryParam["time"].ToString();

                pagination.conditionJson += string.Format(" and to_char(LABOROPERATIONTIME,'yyyy-mm-dd')  = '{0}'", name);


            }
            if (!queryParam["DeptCode"].IsEmpty())
            {
                string deptcode = queryParam["DeptCode"].ToString();

                pagination.conditionJson += string.Format(" and createuserdeptcode  like '{0}%'", deptcode);


            }
            if (!queryParam["type"].IsEmpty())
            {
                string unit = queryParam["type"].ToString();

                pagination.conditionJson += string.Format(" and type  like '%{0}%'", unit);
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<LaborissueEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public LaborissueEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, LaborissueEntity entity)
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
