using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// �� ������ȫ��ʩ�䶯�����
    /// </summary>
    public interface SafetychangeIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SafetychangeEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡ��ȫ��ʩ�䶯̨��
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetLedgerList(Pagination pagination, string queryJson);

        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SafetychangeEntity GetEntity(string keyValue);
        Flow GetFlow(string keyValue, List<string> modulename);

        List<CheckFlowData> GetAppFlowList(string keyValue, List<string> modulename, string flowid, bool isendflow, string workdeptid, string projectid, string specialtytype = "");
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
        void SaveForm(string keyValue, SafetychangeEntity entity);

        DataTable FindTable(string sql);
        #endregion
    }
}
