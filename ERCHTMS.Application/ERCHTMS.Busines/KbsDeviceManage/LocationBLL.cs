using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using ERCHTMS.Service.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.KbsDeviceManage
{
    /// <summary>
    /// �� ������λ���¼��
    /// </summary>
    public class LocationBLL
    {
        private LocationIService service = new LocationService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<LocationEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public LocationEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
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
            return service.GetLocation(lableid, st, et);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, LocationEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="entityList"></param>
        public bool Insert(List<LocationEntity> entityList)
        {
            try
            {
                return service.Insert(entityList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}
