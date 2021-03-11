using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using ERCHTMS.Service.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.KbsDeviceManage
{
    /// <summary>
    /// �� �����豸���߼�¼
    /// </summary>
    public class OfflinedeviceBLL
    {
        private OfflinedeviceIService service = new OfflinedeviceService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OfflinedeviceEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OfflinedeviceEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ��״ͼͳ������
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetTable(int type)
        {
            return service.GetTable(type);
        }

        /// <summary>
        /// ��ѯ�����豸ǰ����
        /// </summary>
        /// <param name="type">�豸���� 0��ǩ 1��վ 2�Ž� 3����ͷ</param>
        /// <param name="Time">1���� 2����</param>
        /// <param name="topNum">ǰ����</param>
        /// <returns></returns>
        public DataTable GetOffTop(int type, int Time, int topNum)
        {
            return service.GetOffTop(type, Time, topNum);
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
        public void SaveForm(string keyValue, OfflinedeviceEntity entity)
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
        #endregion
    }
}
