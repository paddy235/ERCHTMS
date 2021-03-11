using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ERCHTMS.Service.KbsDeviceManage
{
    /// <summary>
    /// �� ������λ���¼��
    /// </summary>
    public class LocationService : RepositoryFactory<LocationEntity>, LocationIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<LocationEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public LocationEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// ����ʱ��λ�ȡ��λ��¼
        /// </summary>
        /// <param name="lableid"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        public List<LocationEntity> GetLocation(string lableid, DateTime st, DateTime et)
        {
            return BaseRepository().IQueryable(it => it.LableID == lableid && it.CreateDate >= st && it.CreateDate <= et).ToList();
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
        public void SaveForm(string keyValue, LocationEntity entity)
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
        /// ������������
        /// </summary>
        /// <param name="entityList"></param>
        public bool Insert(List<LocationEntity> entityList)
        {
            int num = this.BaseRepository().Insert(entityList);
            if (num > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
