using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// �� �������ص�װ��ҵ������Ա��
    /// </summary>
    public class LifthoistpersonBLL
    {
        private LifthoistpersonIService service = new LifthoistpersonService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<LifthoistpersonEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// ��ȡ���ص�װ�����Ա��Ϣ
        /// </summary>
        /// <param name="workid"></param>
        /// <returns></returns>
        public IEnumerable<LifthoistpersonEntity> GetRelateList(string workid)
        {
            return service.GetRelateList(workid);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public LifthoistpersonEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// ֤���Ų����ظ�
        /// </summary>
        /// <param name="CertificateNum">֤����</param>
        /// <param name="keyValue">����</param>
        /// <returns></returns>
        public bool ExistCertificateNum(string CertificateNum, string keyValue)
        {
            return service.ExistCertificateNum(CertificateNum, keyValue);
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
        public void SaveForm(string keyValue, LifthoistpersonEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ɾ�����ص�װ��ҵ��ص���Ա��Ϣ
        /// </summary>
        /// <param name="WorkId"></param>
        public void RemoveFormByWorkId(string WorkId)
        {
            try
            {
                service.RemoveFormByWorkId(WorkId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
