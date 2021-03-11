using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;

using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// �� ������ȫ��������
    /// </summary>
    public interface SafetyAssessmentIService
    {
        #region ��ȡ����
        /// <summary>
        /// ������ȫ���˻���
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        DataTable ExportDataTotal(string time, string deptid);

        /// <summary>
        /// ��ȡ�ڲ�����
        /// </summary>
        /// <returns></returns>
        DataTable GetInDeptData();

        /// <summary>
        /// �ڲ����ſ��˵���
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        DataTable ExportDataInDept(string time, string deptid);

        /// <summary>
        /// �ⲿ���ſ���
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        DataTable ExportDataOutDept(string time, string deptid);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SafetyAssessmentEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SafetyAssessmentEntity GetEntity(string keyValue);

        int GetFormJsontotal(string keyValue);

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        string GetMaxCode();

        /// <summary>
        ///  ��ȡ��ǰ��ɫ������������
        /// </summary>
        /// <returns></returns>
        string GetApplyNum();
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
        void SaveForm(string keyValue, SafetyAssessmentEntity entity);
        #endregion

        #region  ��ǰ��¼���Ƿ���Ȩ����˲���ȡ��һ�����Ȩ��ʵ��
        /// <summary>
        /// ��ǰ��¼���Ƿ���Ȩ����˲���ȡ��һ�����Ȩ��ʵ��
        /// </summary>
        /// <param name="currUser">��ǰ��¼��</param>
        /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
        /// <param name="moduleName">ģ������</param>
        /// <param name="createdeptid">�����˲���ID</param>
        /// <returns>null-��ǰΪ���һ�����,ManyPowerCheckEntity����һ�����Ȩ��ʵ��</returns>
        ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid,string startnum);

        Flow GetAuditFlowData(string keyValue, string urltype);
        #endregion
    }
}
