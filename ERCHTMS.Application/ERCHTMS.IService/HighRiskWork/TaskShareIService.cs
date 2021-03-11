using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public interface TaskShareIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<TaskShareEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        TaskShareEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡ���������б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        DataTable GetDataTable(Pagination page, string queryJson, string authType);

        /// <summary>
        /// ��վ�ලͳ��
        /// </summary>
        /// <param name="sentity"></param>
        /// <returns></returns>
        DataTable QueryStatisticsByAction(StatisticsEntity sentity);
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
        List<PushMessageData> SaveForm(string keyValue, TaskShareEntity entity);

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveOnlyShare(string keyValue, TaskShareEntity entity);
        #endregion
    }
}
