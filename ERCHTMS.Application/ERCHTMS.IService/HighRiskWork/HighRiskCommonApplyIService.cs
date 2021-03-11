using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.HiddenTroubleManage;
using System.Collections;
using ERCHTMS.Code;
using System;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// �� �����߷���ͨ����ҵ����
    /// </summary>
    public interface HighRiskCommonApplyIService
    {
        #region ��ȡ����
        /// <summary>
        /// �õ���ǰ�����
        /// </summary>
        /// <returns></returns>
        object GetMaxCode();
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        IEnumerable<HighRiskCommonApplyEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<HighRiskCommonApplyEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        HighRiskCommonApplyEntity GetEntity(string keyValue);
        /// <summary>
        /// ��ȡ�߷���ͨ��̨��
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetLedgerList(Pagination pagination, string queryJson, Boolean GetOperate = true);
        /// <summary>
        /// ��ȡ�߷���ͨ����ҵ�����б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetPageDataTable(Pagination pagination, string queryJson);

        DataTable GetTable(string sql);

        /// <summary>
        /// ��ȡִ�в���
        /// </summary>
        /// <param name="workdepttype">��ҵ��λ����</param>
        /// <param name="workdept">��ҵ��λ</param>
        /// <param name="projectid">�������ID</param>
        /// <param name="Executedept">ִ�в���</param>
        void GetExecutedept(string workdepttype, string workdept, string projectid, out string Executedept);

        /// <summary>
        /// ��ȡ�����λ
        /// </summary>
        /// <param name="workdept">��ҵ��λ</param>
        /// <param name="outsouringengineerdept"></param>
        void GetOutsouringengineerDept(string workdept, out string outsouringengineerdept);


        List<CheckFlowData> GetAppFlowList(string keyValue, string modulename);

        Flow GetFlow(string keyValue, string modulename);

        /// <summary>
        /// ��ȡ�����������
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        string GetModuleName(HighRiskCommonApplyEntity entity);
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
        PushMessageData SaveForm(string keyValue, string type, HighRiskCommonApplyEntity entity, List<HighRiskRecordEntity> list, List<HighRiskApplyMBXXEntity> mbList);

        void SaveApplyForm(string keyValue, HighRiskCommonApplyEntity entity);

        /// <summary>
        /// ȷ�ϣ����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="state"></param>
        /// <param name="recordData"></param>
        /// <param name="entity"></param>
        /// <param name="aentity"></param>
        PushMessageData SubmitCheckForm(string keyValue, string state, string recordData, HighRiskCommonApplyEntity entity, ScaffoldauditrecordEntity aentity);

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="entity">����</param>
        void UpdateForm(HighRiskCommonApplyEntity entity);

        /// <summary>
        /// �޸�sql���
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int UpdateData(string sql);
        #endregion

        #region ͳ��
        /// <summary>
        /// ����ҵ����ͳ��
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        string GetHighWorkCount(string starttime, string endtime, string deptid, string deptcode);

        /// <summary>
        ///��ҵ����ͳ��(ͳ�Ʊ��)
        /// </summary>
        /// <returns></returns>
        string GetHighWorkList(string starttime, string endtime, string deptid, string deptcode);

        /// <summary>
        /// �¶�����(ͳ��ͼ)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        string GetHighWorkYearCount(string year, string deptid, string deptcode);

        /// <summary>
        /// �¶�����(���)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        string GetHighWorkYearList(string year, string deptid, string deptcode);


        /// <summary>
        /// ��λ�Ա�(ͳ��ͼ)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        string GetHighWorkDepartCount(string starttime, string endtime);

        /// <summary>
        /// ��λ�Ա�(���)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        string GetHighWorkDepartList(string starttime, string endtime);
        #endregion


        #region ��ȡ���ո߷�����ҵ
        /// <summary>
        /// ��ȡ���ո߷�����ҵ(��ҵ̨������ҵ�е�����)
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetTodayWorkList(Pagination pagination, string queryJson);
        #endregion

        #region �ֻ�����ҵͳ��
        DataTable AppGetHighWork(string starttime, string endtime, string deptid, string deptcode);
        #endregion

        #region  �����Ҫ
        bool GetProjectNum(string outProjectId);
        DataTable GetCountByArea(List<string> areaCodes);
        #endregion
    }
}
