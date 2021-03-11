using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util.Extension;
using BSFramework.Util;

namespace ERCHTMS.Service.SystemManage
{
    /// <summary>
    /// �� ��������糧�ύ����˵����
    /// </summary>
    public class OutcommitfileService : RepositoryFactory<OutcommitfileEntity>, OutcommitfileIService
    {
        #region ��ȡ����
        public DataTable GetPageList(Pagination pagination, string queryJson) {
            var queryParam = queryJson.ToJObject();
            DatabaseType dataType = DbHelper.DbType;
            if (!queryParam["Keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.title like'{0}%' ", queryParam["Keyword"].ToString());
            }
            DataTable result = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return result;
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OutcommitfileEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OutcommitfileEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ���ݻ���Code��ѯ�������Ƿ��Ѿ����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public bool GetIsExist(string orgCode) {
            var count = this.BaseRepository().IQueryable().Where(x => x.CreateUserOrgCode == orgCode).ToList().Count;
            if (count == 0) return true;
            else
                return false;
        }

        public OutcommitfileEntity GetEntityByOrgCode(string orgCode) {
           return this.BaseRepository().IQueryable().Where(x => x.CreateUserOrgCode == orgCode).ToList().FirstOrDefault();
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
        public void SaveForm(string keyValue, OutcommitfileEntity entity)
        {
            entity.ID = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                var oldEntity = this.BaseRepository().FindEntity(keyValue);
                if (oldEntity != null)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
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
