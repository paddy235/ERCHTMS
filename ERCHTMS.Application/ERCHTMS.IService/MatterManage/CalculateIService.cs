using ERCHTMS.Entity.MatterManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.MatterManage
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public interface CalculateIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<CalculateEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        CalculateEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡ�û���Ȩ��¼
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        UserEmpowerRecordEntity GetUserRecord(string keyValue);

        /// <summary>
        /// ��ȡ���³��ؼ�����Ϣ
        /// </summary>
        /// <param name="keyValue">���ص���</param>
        /// <returns></returns>
        CalculateEntity GetNewEntity(string keyValue);



        /// <summary>
        ///����δ��������
        /// </summary>
        /// <returns>�����б�</returns>
        CalculateEntity GetEntranceTicket(string carNo);



        
        /// <summary>
        /// ��ȡ��¼���������¼ʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        CalculateDetailedEntity GetAppDetailedEntity(string keyValue);

        /// <summary>
        /// �б��ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// �б��ҳ����
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetNewPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// ��ȡ����ͳ���б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetCountPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// ��ȡ�ذ�Ա�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetPageUserList(Pagination pagination, string queryJson, string res);

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
        void SaveForm(string keyValue, CalculateEntity entity);

        void SaveAppForm(string keyValue, CalculateEntity entity);
        void SaveWeightBridgeDetail(string keyValue, CalculateDetailedEntity entity);

        /// <summary>
        /// �����û���Ȩ��Ϣ
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveUserForm(string keyValue, UserEmpowerRecordEntity entity);

      

        #endregion
    }
}
