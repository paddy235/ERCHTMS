using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// �� �����߷�����ҵ�������
    /// </summary>
    public interface HighRiskApplyIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        IEnumerable<HighRiskApplyEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<HighRiskApplyEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        HighRiskApplyEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetPageDataTable(Pagination pagination, string queryJson);

        /// <summary>
        /// ��ȡ���������ҵ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetSelectDataTable(Pagination pagination, string queryJson);

        /// <summary>
        /// ��ȡ���(��)�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetVerifyPageTableJson(Pagination pagination, string queryJson);

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
        void SaveForm(string keyValue, HighRiskApplyEntity entity);
        #endregion

        #region ͳ��
        /// <summary>
        /// ��ȡ���������ҵ�б�(ͳ����ת)
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetStatisticsWorkTable(Pagination pagination, string queryJson);

        /// <summary>
        /// ����ҵ����ͳ��
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        string GetHighWorkCount(string starttime, string endtime, string deptid, string deptcode);

        /// <summary>
        ///��ҵ����ͳ��(ͳ�Ʊ��)
        /// </summary>
        /// <returns></returns>
        string GetHighWorkList(string starttime, string endtime, string deptid, string deptcode);

        /// <summary>
        /// �¶�����(ͳ��ͼ)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        string GetHighWorkYearCount(string year, string deptid, string deptcode);

        /// <summary>
        /// �¶�����(���)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        string GetHighWorkYearList(string year, string deptid, string deptcode);
        #endregion

        #region �ƶ���
        /// <summary>
        /// ����ҵ����ͳ��
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        DataTable GetWorkTypeInfo(string starttime, string endtime, string deptid, string deptcode);


        /// <summary>
        /// �¶�����
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        DataTable GetWorkYearCount(string year, string deptid, string deptcode);
        #endregion
    }
}
