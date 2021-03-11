using ERCHTMS.Entity.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.KbsDeviceManage
{
    /// <summary>
    /// �� ������ǩ��������־
    /// </summary>
    public interface LableonlinelogIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<LableonlinelogEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        LableonlinelogEntity GetEntity(string keyValue);

        /// <summary>
        /// ���ݱ�ǩID��ȡ���߱�ǩ
        /// </summary>
        /// <param name="LableId"></param>
        /// <returns></returns>
        LableonlinelogEntity GetOnlineEntity(string LableId);
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
        void SaveForm(string keyValue, LableonlinelogEntity entity);

        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool SaveStatus(LableonlinelogEntity entity);

        #endregion
    }
}
