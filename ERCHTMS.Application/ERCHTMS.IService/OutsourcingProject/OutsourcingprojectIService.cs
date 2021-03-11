using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// �� ���������λ������Ϣ��
    /// </summary>
    public interface OutsourcingprojectIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<OutsourcingprojectEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        OutsourcingprojectEntity GetEntity(string keyValue);
        /// <summary>
        /// ���������λId��ȡ�����λ������Ϣ
        /// </summary>
        /// <param name="outProjectId"></param>
        /// <returns></returns>
        OutsourcingprojectEntity GetInfo(string outProjectId);
        DataTable GetPageList(Pagination pagination, string queryJson);

        string StaQueryList(string queryJson);
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
        void SaveForm(string keyValue, OutsourcingprojectEntity entity);
        #endregion
    }
}
