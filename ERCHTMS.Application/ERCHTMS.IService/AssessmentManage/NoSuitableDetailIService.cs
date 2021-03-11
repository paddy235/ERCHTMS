using ERCHTMS.Entity.AssessmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.AssessmentManage
{
    /// <summary>
    /// �� ������������ɸѡ
    /// </summary>
    public interface NoSuitableDetailIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<NoSuitableDetailEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        NoSuitableDetailEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetPageListJson(Pagination pagination, string queryJson);

        /// <summary>
        /// ���ݼƻ�id��ȡ����
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        DataTable GetDetailInfo(string planid);

        /// <summary>
        /// ���������С���б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetAllDetailPage(Pagination pagination);

         /// <summary>
        /// ���ݼƻ�id��С��ڵ�id��ȡ������������
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        NoSuitableDetailEntity GetNoSuitByPlanOrChapID(string planid, string chapterid);
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
        void SaveForm(string keyValue, NoSuitableDetailEntity entity);
        #endregion
    }
}
