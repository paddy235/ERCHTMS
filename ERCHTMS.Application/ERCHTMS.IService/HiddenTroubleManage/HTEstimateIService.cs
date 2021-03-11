using ERCHTMS.Entity.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������������
    /// </summary>
    public interface HTEstimateIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<HTEstimateEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        HTEstimateEntity GetEntity(string keyValue);
        /// <summary>
        /// ���ݱ����ȡ��Ӧ����Ч������ʵ��
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        HTEstimateEntity GetEntityByHidCode(string hidCode);

        IEnumerable<HTEstimateEntity> GetHistoryList(string hidCode);
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValue);

        void RemoveFormByCode(string hidcode);
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string keyValue, HTEstimateEntity entity);
        #endregion
    }
}
