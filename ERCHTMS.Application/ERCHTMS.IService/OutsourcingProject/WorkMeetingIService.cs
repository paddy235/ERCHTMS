using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// �� �������չ���
    /// </summary>
    public interface WorkMeetingIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<WorkMeetingEntity> GetList(string queryJson);
        DataTable GetTable(string sql);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        WorkMeetingEntity GetEntity(string keyValue);
         /// <summary>
        /// ���ݵ�ǰ��¼�˻�ȡδ�ύ������
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        DataTable GetNotCommitData(string userid);
        /// <summary>
        /// ��ȡ���տ��������ʱ������
        /// </summary>
        /// <returns></returns>
        int GetTodayTempProject(Operator curUser);
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
        void SaveForm(string keyValue, WorkMeetingEntity entity);

        void SaveWorkMeetingForm(string keyValue, WorkMeetingEntity entity, List<WorkmeetingmeasuresEntity> list,string ids);
        #endregion
    }
}
