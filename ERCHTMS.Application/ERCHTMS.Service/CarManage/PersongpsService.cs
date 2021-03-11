using System;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// �� ������ԱGPS������
    /// </summary>
    public class PersongpsService : RepositoryFactory<PersongpsEntity>, PersongpsIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<PersongpsEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable(it => it.VID == queryJson).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public PersongpsEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ѯ��GPS�豸�Ƿ�ռ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool GetGps(string id, string gpsid)
        {
            string sql = string.Format(@"select count(id) as cou from bis_persongps where STATE=0 and VID!='{0}' and gpsid='{1}'", id, gpsid);
            int i = Convert.ToInt32(BaseRepository().FindObject(sql));
            return i > 0;
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
        public void SaveForm(string keyValue, PersongpsEntity entity)
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
