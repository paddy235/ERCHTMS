using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using System.Data;
using ERCHTMS.Code;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// �� ����ʹ������ˮ
    /// </summary>
    public interface FireWaterIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        DataTable GetList(Pagination page, string queryJson, string authType, Operator user);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        FireWaterEntity GetEntity(string keyValue);
        FireWaterCondition GetConditionEntity(string fireWaterId);

        /// <summary>
        /// ��ȡִ���������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        IList<FireWaterCondition> GetConditionList(string keyValue);
        #endregion

        #region ��ȡ����ˮʹ��̨��
        /// <summary>
        /// ��ȡ����ˮʹ��̨��
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetLedgerList(Pagination pagination, string queryJson, Operator user);
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
        void SaveForm(string keyValue, FireWaterModel model);

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string keyValue, FireWaterEntity entity);


        /// <summary>
        /// ����ҵ�����˱�
        /// </summary>
        /// <param name="fireWaterEntity">ҵ������ʵ��</param>
        /// <param name="auditEntity">��˱�ʵ��</param>
        void UpdateForm(FireWaterEntity fireWaterEntity, ScaffoldauditrecordEntity auditEntity);
        /// <summary>
        /// �ύִ�����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        void SubmitCondition(string keyValue, FireWaterCondition entity);

        /// <summary>
        /// ��ȡAPP����ͼ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        List<CheckFlowData> GetAppFlowList(string keyValue, string modulename);

        /// <summary>
        /// �õ�����ͼ
        /// </summary>
        /// <param name="keyValue">ҵ���ID</param>
        /// <param name="modulename">�����ģ����</param>
        /// <returns></returns>
        Flow GetFlow(string keyValue, string modulename);
        #endregion
    }
}
