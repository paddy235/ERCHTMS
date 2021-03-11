using ERCHTMS.Entity.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public interface HTApprovalIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<HTApprovalEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        HTApprovalEntity GetEntity(string keyValue);

        /// <summary>
        /// ͨ�����������ȡ
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        HTApprovalEntity GetEntityByHidCode(string hidCode);
        /// <summary>
        /// �������������ȡTable
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        DataTable GetDataTableByHidCode(string hidCode);

        IEnumerable<HTApprovalEntity> GetHistoryList(string hidCode);
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
        void SaveForm(string keyValue, HTApprovalEntity entity);
        #endregion
    }
}
