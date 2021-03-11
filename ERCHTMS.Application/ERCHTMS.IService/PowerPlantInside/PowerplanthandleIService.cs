using ERCHTMS.Entity.PowerPlantInside;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.IService.PowerPlantInside
{
    /// <summary>
    /// �� �����¹��¼�����
    /// </summary>
    public interface PowerplanthandleIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<PowerplanthandleEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        PowerplanthandleEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// ��ǰ��¼���Ƿ���Ȩ����˲���ȡ��һ�����Ȩ��ʵ��
        /// </summary>
        /// <param name="currUser">��ǰ��¼��</param>
        /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
        /// <param name="moduleName">ģ������</param>
        /// <param name="createdeptid">�����˲���ID</param>
        /// <returns>null-��ǰΪ���һ�����,ManyPowerCheckEntity����һ�����Ȩ��ʵ��</returns>
        ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid);

        /// <summary>
        /// �¹��¼��������
        /// </summary>
        /// <returns></returns>
        List<string> ToAuditPowerHandle();

        DataTable GetAuditInfo(string keyValue, string modulename);


        DataTable GetTableBySql(string sql);
        /// <summary>
        /// ��ȡ����ͼ������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        DataTable GetReformInfo(string keyValue);

        /// <summary>
        /// ��ȡ����ͼ������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        DataTable GetCheckInfo(string keyValue, string modulename);

        string GetUserName(string flowdeptid, string flowrolename, string type = "", string specialtytype = "");

        List<CheckFlowData> GetAppFlowList(string keyValue);

        List<CheckFlowData> GetAppFullFlowList(string keyValue);
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
        void SaveForm(string keyValue, PowerplanthandleEntity entity);

        /// <summary>
        /// �����¹��¼���¼״̬
        /// </summary>
        /// <param name="keyValue"></param>
        void UpdateApplyStatus(string keyValue);
        #endregion
    }
}
