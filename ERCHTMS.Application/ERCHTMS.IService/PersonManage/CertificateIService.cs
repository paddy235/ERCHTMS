using ERCHTMS.Entity.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.PersonManage
{
    /// <summary>
    /// �� ������Ա֤��
    /// </summary>
    public interface CertificateIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<CertificateEntity> GetList(string queryJson, Pagination pag = null);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        CertificateEntity GetEntity(string keyValue);

        /// <summary>
        /// ������Ա��֤��
        /// </summary>
        /// <param name="sum">��Ա������</param>
        /// <param name="deptCode">��ǰ�û��������ű���</param>
        /// <param name="certType">֤�����</param>
        /// <returns></returns>
        decimal GetCertPercent(int sum, string deptCode, string certType);
        /// <summary>
        /// ���ݵ�ǰ�û���Ȩ�޷�Χ��ȡ������ļ������ں��ѹ��ڵ�֤����Ϣ
        /// </summary>
        /// <param name="where">����Ȩ�޷�Χ</param>
        /// <returns></returns>
        Dictionary<string, string> GetOverdueCertList(string where);
        /// <summary>
        /// ��ȡ��Ա֤��
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        DataTable GetCertList(string userId);

        /// <summary>
        /// ��ȡ֤�������¼
        /// </summary>
        /// <param name="certId"></param>
        /// <returns></returns>
        DataTable GetAuditList(string certId);

        /// <summary>
        /// ��ȡ������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        CertAuditEntity GetAuditEntity(string keyValue);
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
        bool SaveForm(string keyValue, CertificateEntity entity);
        /// <summary>
        /// �������޸�֤�鸴���¼
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool SaveCertAudit(CertAuditEntity entity);
        /// <summary>
        /// ɾ��֤�鸴���¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        bool RemoveCertAudit(string keyValue);
        #endregion
    }
}
