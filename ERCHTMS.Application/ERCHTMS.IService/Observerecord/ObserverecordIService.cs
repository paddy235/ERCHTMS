using ERCHTMS.Entity.Observerecord;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.Observerecord
{
    /// <summary>
    /// �� �����۲��¼��
    /// </summary>
    public interface ObserverecordIService
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
        IEnumerable<ObserverecordEntity> GetList();
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        ObserverecordEntity GetEntity(string keyValue);
        /// <summary>
        /// ���ݹ۲��¼Id��ȡ�۲����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetObsTypeData(string keyValue);
        /// <summary>
        /// ��ȡ��ȫ��Ϊ�벻��ȫ��Ϊռ��ͳ��
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        string GetSafetyStat(string deptCode, string year = "", string quarter = "",string month="");
        /// <summary>
        /// ��ȡ����ȫ��������ͼ
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        string GetQsStat(string deptCode, string year = "");
        /// <summary>
        /// ��ȡ�۲�����Ա�ͼ
        /// </summary>
        /// <param name="deptCode">��λCode</param>
        /// <param name="year">��</param>
        /// <param name="quarter">����</param>
        /// <param name="month">�¶�</param>
        /// <param name="issafety">issafety 0 ����ȫ��Ϊ 1 ��ȫ��Ϊ</param>
        /// <returns></returns>
        string GetUntiDbStat(string deptCode, string issafety, string year = "", string quarter = "", string month = "");
        DataTable GetTable(string sql);
        /// <summary>
        /// ���ݹ۲�ƻ�Id������ֽ�Id��ѯ�Ƿ�����˹۲��¼
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="planfjid"></param>
        /// <returns></returns>
        bool GetObsRecordByPlanIdAndFjId(string planid, string planfjid);
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
        void SaveForm(string keyValue, ObserverecordEntity entity, List<ObservecategoryEntity> listCategory, List<ObservesafetyEntity> listSafety);
        #endregion
    }
}
