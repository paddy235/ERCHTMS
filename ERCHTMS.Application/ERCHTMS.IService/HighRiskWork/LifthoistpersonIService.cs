using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// �� �������ص�װ��ҵ������Ա��
    /// </summary>
    public interface LifthoistpersonIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<LifthoistpersonEntity> GetList(string queryJson);

        /// <summary>
        /// ��ȡ���ص�װ�����Ա��Ϣ
        /// </summary>
        /// <param name="workid"></param>
        /// <returns></returns>
        IEnumerable<LifthoistpersonEntity> GetRelateList(string workid);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        LifthoistpersonEntity GetEntity(string keyValue);

        /// <summary>
        /// ֤���Ų����ظ�
        /// </summary>
        /// <param name="CertificateNum">֤����</param>
        /// <param name="keyValue">����</param>
        /// <returns></returns>
        bool ExistCertificateNum(string CertificateNum, string keyValue);
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
        void SaveForm(string keyValue, LifthoistpersonEntity entity);

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// ɾ�����ص�װ��ҵ��ص���Ա��Ϣ
        /// </summary>
        /// <param name="WorkId"></param>
        void RemoveFormByWorkId(string WorkId);
        #endregion
    }
}
