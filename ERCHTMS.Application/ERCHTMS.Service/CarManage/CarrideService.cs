using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// �� �����೵�ϳ���¼��
    /// </summary>
    public class CarrideService : RepositoryFactory<CarrideEntity>, CarrideIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<CarrideEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public CarrideEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ�����������Ա
        /// </summary>
        /// <param name="lid"></param>
        /// <returns></returns>
        public string GetCarRide(string lid)
        {
            string sql = string.Format(@"select LISTAGG(username,',') WITHIN GROUP (ORDER BY username) as us from
            (select realname as username,lid from BIS_CARRIDE ride left join base_user use on use.userid=ride.createuserid) f where lid='{0}' group by lid",
                lid);
            object name=BaseRepository().FindObject(sql);
            if (name != null)
            {
                return name.ToString();
            }
            else
            {
                return "";
            }
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
        public void SaveForm(string keyValue, CarrideEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Delete(it =>
                    it.Status == 0 && it.CarNo == entity.CarNo && it.CreateUserId == entity.CreateUserId);
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
