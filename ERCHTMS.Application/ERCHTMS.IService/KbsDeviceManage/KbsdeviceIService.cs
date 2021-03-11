using ERCHTMS.Entity.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.KbsDeviceManage
{
    /// <summary>
    /// �� ��������ʲ�Ž�����
    /// </summary>
    public interface KbsdeviceIService
    {
        #region ��ȡ����

        /// <summary>
        /// ��ȡȫ���б�����
        /// </summary>
        /// <returns>���ط�ҳ�б�</returns>
        List<KbsdeviceEntity> GetPageList();
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<KbsdeviceEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        KbsdeviceEntity GetEntity(string keyValue);

        /// <summary>
        /// ����״̬��ȡ����ͷ����
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        int GetDeviceNum(string status);
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
        void SaveForm(string keyValue, KbsdeviceEntity entity);

        /// <summary>
        /// �ӿ��޸�״̬�÷���
        /// </summary>
        /// <param name="entity"></param>
        void UpdateState(KbsdeviceEntity entity);

        #endregion
    }
}
