using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// �� �������֤
    /// </summary>
    public interface LifthoistcertIService
    {
        #region ��ȡ����
         /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        DataTable GetList(Pagination page, LifthoistSearchModel search);

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        LifthoistcertEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, LifthoistcertEntity entity);

        /// <summary>
        /// ��˸���
        /// </summary>
        /// <param name="jobEntity">ƾ��֤ʵ��</param>
        /// <param name="auditEntity">���ʵ��</param>
        void ApplyCheck(LifthoistcertEntity certEntity, LifthoistauditrecordEntity auditEntity);
        #endregion
    }
}
