using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// �� �������ص�װ��ҵ
    /// </summary>
    public interface LifthoistjobIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        DataTable GetList(Pagination page, LifthoistSearchModel search);

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        DataTable getTempEquipentList(Pagination page, LifthoistSearchModel search);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        LifthoistjobEntity GetEntity(string keyValue);

        /// <summary>
        /// �õ�����ͼ
        /// </summary>
        /// <param name="keyValue">ҵ���ID</param>
        /// <param name="modulename">�����ģ����</param>
        /// <returns></returns>
        Flow GetFlow(string keyValue, string modulename);

        List<CheckFlowData> GetAppFlowList(string keyValue, string modulename);
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
        void SaveForm(string keyValue, LifthoistjobEntity entity);

        /// <summary>
        /// ��˸���
        /// </summary>
        /// <param name="jobEntity">���ص�װ��ҵʵ��</param>
        /// <param name="auditEntity">���ʵ��</param>
        void ApplyCheck(LifthoistjobEntity jobEntity, LifthoistauditrecordEntity auditEntity);
        #endregion
    }
}
