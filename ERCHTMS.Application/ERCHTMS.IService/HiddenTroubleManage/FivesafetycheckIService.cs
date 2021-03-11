using ERCHTMS.Entity.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.IService.HiddenTroubleManage
{
    /// <summary>
    /// �� �����嶨��ȫ���
    /// </summary>
    public interface FivesafetycheckIService
    {
        #region ��ȡ����
        /// <summary>
        /// �������ƺͲ��ż���,���������ƵĴ���
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetDeptByName(string name);
        /// <summary>
        /// ���ݼ�����ͱ�Ų�ѯ��ҳ
        /// </summary>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        DataTable DeskTotalByCheckType(string itemcode);

        /// <summary>
        /// ���ذ�ȫ���˲�ͬ���ʹ�����������������
        /// </summary>
        /// <param name="fivetype">�������</param>
        /// <param name="istopcheck"> 0:�ϼ���˾��� 1����˾��ȫ���</param>
        /// <param name="type"> 0:������̣�1������  2������</param>
        /// <returns></returns>
        string GetApplyNum(string fivetype, string istopcheck, string type);

        DataTable GetInfoBySql(string sql);
        DataTable ExportAuditTotal(string keyvalue);
        Flow GetAuditFlowData(string keyValue, string urltype);

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<FivesafetycheckEntity> GetList(string queryJson);

        IEnumerable<UserEntity> GetStepDept(ManyPowerCheckEntity powerinfo, string id);

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetPageListJson(Pagination pagination, string queryJson);

        /// <summary>
        ///  ��ȡ��������б��ҳ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetAuditListJson(Pagination pagination, string queryJson);

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        FivesafetycheckEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, FivesafetycheckEntity entity);
        #endregion
    }
}
