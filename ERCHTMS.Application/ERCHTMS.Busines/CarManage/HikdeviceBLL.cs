using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using ERCHTMS.Service.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.Busines.CarManage
{
    /// <summary>
    /// �� �����Ž��豸����
    /// </summary>
    public class HikdeviceBLL
    {
        private HikdeviceIService service = new HikdeviceService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HikdeviceEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HikdeviceEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HikdeviceEntity GetDeviceEntity(string HikID)
        {
            return service.GetDeviceEntity(HikID);
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
        public void SaveForm(string keyValue, HikdeviceEntity entity)
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
        ///  �����豸�������� ����ȡ�����������е��豸
        /// </summary>
        /// <param name="areaName"></param>
        /// <returns></returns>
        public List<HikdeviceEntity> GetDeviceByArea(string areaName)
        {
            return service.GetDeviceByArea(areaName);
        }

        /// <summary>
        /// ��ȡ���е��豸����
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataItemEntity> GetDeviceArea()
        {
            return service.GetDeviceArea();
        }
        #endregion
    }
}
