using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Code;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// �� ����������������Ϣ��
    /// </summary>
    public interface AptitudeinvestigateinfoIService
    {
        #region ��ȡ����
        DataTable GetPageList(Pagination pagination, string queryJson);
     
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<AptitudeinvestigateinfoEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        AptitudeinvestigateinfoEntity GetEntity(string keyValue);
        /// <summary>
        /// ��������ID��ȡ������Ϣ
        /// </summary>
        /// <param name="engineerid"></param>
        /// <returns></returns>
        AptitudeinvestigateinfoEntity GetEntityByOutEngineerId(string engineerid);
        /// <summary>
        /// ���������λId��ȡ���һ�ε�λ������Ϣ
        /// </summary>
        /// <param name="outprojectId">��λId</param>
        /// <returns></returns>
        AptitudeinvestigateinfoEntity GetListByOutprojectId(string outprojectId);

        /// <summary>
        /// �����������Id��ȡ���һ�ε�λ������Ϣ
        /// </summary>
        /// <param name="outengineerId"></param>
        /// <returns></returns>
        AptitudeinvestigateinfoEntity GetListByOutengineerId(string outengineerId);
        /// <summary>
        /// ��ѯ�������ͼ
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="urlType">��ѯ���ͣ�1 ��λ���� 2 ��Ա���� 3 �����豸���� 4 �綯/��ȫ���������� 5�������� 6�볧��� 7��������</param>
        /// <returns></returns>
        Flow GetAuditFlowData(string keyValue, string urltype);
        /// <summary>
        /// ��ѯ�������ͼ-�ֻ���ʹ��
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="urltype">��ѯ���ͣ�1 ��λ���� 2 ��Ա���� 3 �����豸���� 4 �綯/��ȫ���������� 5�������� 6�볧��� 7�������� 8�ճ�����</param>
        /// <returns></returns>
        List<CheckFlowData> GetAppFlowList(string keyValue, string urltype, Operator currUser);

        List<CheckFlowList> GetAppCheckFlowList(string keyValue, string urltype, Operator currUser);
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
        void SaveForm(string keyValue, AptitudeinvestigateinfoEntity entity);
        #endregion
    }
}
