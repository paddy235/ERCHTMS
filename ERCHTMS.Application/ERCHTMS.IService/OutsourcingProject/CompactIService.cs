using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// �� ������ͬ
    /// </summary>
    public interface CompactIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        DataTable GetList(Pagination pagination, string queryJson);
        IEnumerable<CompactEntity> GetList();
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        DataTable GetEntity(string keyValue);
        /// <summary>
        /// ��������Id��ȡ��ͬ����
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        DataTable GetComoactTimeByProjectId(string projectId);

        #region ��ȡ�����µĺ�ͬ��Ϣ
        /// <summary>
        /// ��ȡ�����µĺ�ͬ��Ϣ
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        List<CompactEntity> GetListByProjectId(string projectId);
        #endregion

        object GetCompactProtocol(string keyValue);

        object GetLastCompactProtocol(string keyValue);
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
        void SaveForm(string keyValue, CompactEntity entity);
        #endregion
    }
}
