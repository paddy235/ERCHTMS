using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.KbsDeviceManage
{
    /// <summary>
    /// �� ��������ʲ�Ž�����
    /// </summary>
    public class KbsdeviceService : RepositoryFactory<KbsdeviceEntity>, KbsdeviceIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡȫ���б�����
        /// </summary>
        /// <returns>���ط�ҳ�б�</returns>
        public List<KbsdeviceEntity> GetPageList()
        {
            return this.BaseRepository().IQueryable().OrderByDescending(it => it.CreateDate).ToList();

        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<KbsdeviceEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public KbsdeviceEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ����״̬��ȡ����ͷ����
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public int GetDeviceNum(string status)
        {
            return this.BaseRepository().IQueryable(it => it.State == status).Count();
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
        public void SaveForm(string keyValue, KbsdeviceEntity entity)
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

        /// <summary>
        /// �ӿ��޸�״̬�÷���
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateState(KbsdeviceEntity entity)
        {
            this.BaseRepository().Update(entity);
        }

        #endregion
    }
}
