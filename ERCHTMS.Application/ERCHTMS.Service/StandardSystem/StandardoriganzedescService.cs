using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.IService.StandardSystem;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.StandardSystem
{
    /// <summary>
    /// �� ������׼����֯����������
    /// </summary>
    public class StandardoriganzedescService : RepositoryFactory<StandardoriganzedescEntity>, StandardoriganzedescIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<StandardoriganzedescEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public StandardoriganzedescEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ���ݱ�׼����֯������ȡʵ��
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public StandardoriganzedescEntity GetEntityByType(string type)
        {
            return this.BaseRepository().IQueryable().ToList().Where(a => a.ORIGANZETYPE == type).FirstOrDefault();
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
        public void SaveForm(string keyValue, StandardoriganzedescEntity entity)
        {
            if (!string.IsNullOrEmpty(entity.ID))
            {
                entity.Modify(entity.ID);
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
