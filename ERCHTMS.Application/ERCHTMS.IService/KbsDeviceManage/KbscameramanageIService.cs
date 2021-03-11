using ERCHTMS.Entity.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.KbsDeviceManage
{
    /// <summary>
    /// �� ��������ʲ����ͷ����
    /// </summary>
    public interface KbscameramanageIService
    {
        #region ��ȡ����

        /// <summary>
        /// ��ȡȫ���б�����
        /// </summary>
        /// <returns>���ط�ҳ�б�</returns>
        List<KbscameramanageEntity> GetPageList();
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<KbscameramanageEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        KbscameramanageEntity GetEntity(string keyValue);

        /// <summary>
        /// ����״̬��ȡ����ͷ����
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        int GetCameraNum(string status);
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
        void SaveForm(string keyValue, KbscameramanageEntity entity);

        /// <summary>
        /// �ӿ��޸�״̬�÷���
        /// </summary>
        /// <param name="entity"></param>
        void UpdateState(KbscameramanageEntity entity);
        bool UniqueCheck(string cameraId);

        #endregion
    }
}
