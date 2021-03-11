using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// �� ������Σ����ҵ���/������
    /// </summary>
    public interface HighRiskCheckIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<HighRiskCheckEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        HighRiskCheckEntity GetEntity(string keyValue);

        /// <summary>
        /// ���������idɾ������
        /// </summary>
        int Remove(string workid);

        /// <summary>
        /// ��������id��ȡ�����Ϣ[������û���]
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        IEnumerable<HighRiskCheckEntity> GetCheckListInfo(string approveid);

        /// <summary>
        /// ��������id��ȡû��˵�����
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        int GetNoCheckNum(string approveid);

          /// <summary>
        /// ��������id�͵�ǰ��¼�˻�ȡ���(��)��¼
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        HighRiskCheckEntity GetNeedCheck(string approveid);

         /// <summary>
        /// ��������id��ȡ������Ϣ[������û���]
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        HighRiskCheckEntity GetApproveInfo(string approveid);
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
        void SaveForm(string keyValue, HighRiskCheckEntity entity);
        #endregion
    }
}
