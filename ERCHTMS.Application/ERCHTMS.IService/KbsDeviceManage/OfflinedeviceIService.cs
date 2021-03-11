using ERCHTMS.Entity.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.KbsDeviceManage
{
    /// <summary>
    /// �� �����豸���߼�¼
    /// </summary>
    public interface OfflinedeviceIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<OfflinedeviceEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        OfflinedeviceEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡ��״ͼͳ������
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DataTable GetTable(int type);

        /// <summary>
        /// ��ѯ�����豸ǰ����
        /// </summary>
        /// <param name="type">�豸���� 0��ǩ 1��վ 2�Ž� 3����ͷ</param>
        /// <param name="Time">1���� 2����</param>
        /// <param name="topNum">ǰ����</param>
        /// <returns></returns>
        DataTable GetOffTop(int type, int Time, int topNum);
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string keyValue, OfflinedeviceEntity entity);
        #endregion
    }
}
