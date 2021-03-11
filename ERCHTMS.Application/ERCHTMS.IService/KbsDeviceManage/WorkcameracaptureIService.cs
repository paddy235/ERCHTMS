using ERCHTMS.Entity.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.KbsDeviceManage
{
    /// <summary>
    /// �� ����������Աץ�ļ�¼��
    /// </summary>
    public interface WorkcameracaptureIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<WorkcameracaptureEntity> GetList(string queryJson);
        /// <summary>
        /// ���ݹ��������ѯץ��ͼƬ
        /// </summary>
        /// <param name="workid"></param>
        /// <returns></returns>
        List<WorkcameracaptureEntity> GetCaptureList(string workid, string userid, string cameraid);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        WorkcameracaptureEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, WorkcameracaptureEntity entity);
        #endregion
    }
}
