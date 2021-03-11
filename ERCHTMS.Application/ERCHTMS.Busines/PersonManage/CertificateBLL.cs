using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using ERCHTMS.Service.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.PersonManage
{
    /// <summary>
    /// �� ������Ա֤��
    /// </summary>
    public class CertificateBLL
    {
        private CertificateIService service = new CertificateService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<CertificateEntity> GetList(string queryJson, Pagination pag = null)
        {
            return service.GetList(queryJson, pag);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public CertificateEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ������Ա��֤��
        /// </summary>
        /// <param name="sum">��Ա������</param>
        /// <param name="deptCode">��ǰ�û��������ű���</param>
        /// <param name="certType">֤�����</param>
        /// <returns></returns>
        public decimal GetCertPercent(int sum, string deptCode, string certType)
        {
            return service.GetCertPercent(sum, deptCode, certType);
        }
        /// <summary>
        /// ���ݵ�ǰ�û���Ȩ�޷�Χ��ȡ������ļ������ں��ѹ��ڵ�֤����Ϣ
        /// </summary>
        /// <param name="where">����Ȩ�޷�Χ</param>
        /// <returns></returns>
        public Dictionary<string, string> GetOverdueCertList(string where)
        {
            return service.GetOverdueCertList(where);
        }
        /// <summary>
        /// ��ȡ��Ա֤��
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetCertList(string userId)
        {
            return service.GetCertList(userId);
        }
        /// <summary>
        /// ��ȡ֤�������¼
        /// </summary>
        /// <param name="certId"></param>
        /// <returns></returns>
        public DataTable GetAuditList(string certId)
        {
            return service.GetCertList(certId);
        }
        /// <summary>
        /// ��ȡ������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public CertAuditEntity GetAuditEntity(string keyValue)
        {
            return service.GetAuditEntity(keyValue);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public bool SaveForm(string keyValue, CertificateEntity entity)
        {
            try
            {
                return service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// �������޸�֤�鸴���¼
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool SaveCertAudit(CertAuditEntity entity)
        {
            return service.SaveCertAudit(entity);
        }
        /// <summary>
        /// ɾ��֤�鸴���¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public bool RemoveCertAudit(string keyValue)
        {
            return service.RemoveCertAudit(keyValue);
        }
        #endregion
    }
}
