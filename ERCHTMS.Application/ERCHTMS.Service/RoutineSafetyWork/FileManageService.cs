using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.IService.RoutineSafetyWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.RoutineSafetyWork
{
    /// <summary>
    /// �� �����ļ�����
    /// </summary>
    public class FileManageService : RepositoryFactory<FileManageEntity>, FileManageIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<FileManageEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public FileManageEntity GetEntity(string keyValue)
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
        /// ����������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, FileManageEntity entity)
        {
            bool b = true;
            entity.ID = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                FileManageEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se != null)
                {
                    b = false;
                }
            }
            if (b)
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
            else {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
        }
        #endregion
    }
}