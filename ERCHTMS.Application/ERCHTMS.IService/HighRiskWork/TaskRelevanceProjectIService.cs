using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// �� �����Ѽ��ļ����Ŀ
    /// </summary>
    public interface TaskRelevanceProjectIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<TaskRelevanceProjectEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        TaskRelevanceProjectEntity GetEntity(string keyValue);

          /// <summary>
        /// ���ݼල�����ȡ�Ѽ����Ŀ
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        IEnumerable<TaskRelevanceProjectEntity> GetEndCheckInfo(string superviseid);

        /// <summary>
        /// ���ݼ����Ŀid�ͼල����id��ȡ��Ϣ
        /// </summary>
        /// <returns></returns>
        TaskRelevanceProjectEntity GetCheckResultInfo(string checkprojectid, string superviseid);

         /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetPageDataTable(Pagination pagination);

        /// <summary>
        /// ���ݼලid��ȡ������Ϣ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetTaskHiddenInfo(string superviseid);
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
        void SaveForm(string keyValue, TaskRelevanceProjectEntity entity);
        #endregion
    }
}
