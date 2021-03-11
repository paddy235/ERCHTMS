using ERCHTMS.Entity.Observerecord;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.IService.Observerecord
{
    /// <summary>
    /// �� �����۲�ƻ�
    /// </summary>
    public interface ObsplanIService
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
        IEnumerable<ObsplanEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        ObsplanEntity GetEntity(string keyValue);
        /// <summary>
        /// ���ݹ۲�ƻ�Id������ֽ�Id��ȡ��Ӧ��Ϣ--�˴���ѯ���Ƿ����������--BIS_OBSPLAN_FB
        /// </summary>
        /// <param name="PlanId">�ƻ�id </param>
        /// <param name="PlanFjId">����ֽ�Id</param>
        /// <returns></returns>
        DataTable GetPlanById(string PlanId, string PlanFjId,string month);
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
        void SaveForm(string keyValue, ObsplanEntity entity);
        bool CommitEhsData(Operator currUser);
        /// <summary>
        /// ��ѯ����ȸù۲�ƻ���Щ�·ݽ����˹۲��¼
        /// </summary>
        /// <param name="planId">�ƻ�Id</param>
        /// <param name="planfjid">�ƻ�����ֽ�Id</param>
        /// <param name="year">���</param>
        /// <returns></returns>
        DataTable GetObsRecordIsExist(string planId, string planfjid, string year);
        /// <summary>
        /// ����ģ������
        /// </summary>
        /// <param name="obsplan"></param>
        /// <param name="obsplanwork"></param>
        void InsertImportData(List<ObsplanEntity> obsplan, List<ObsplanworkEntity> obsplanwork);
        void InsertImportData(List<ObsplanEHSEntity> obsplan, List<ObsplanworkEntity> obsplanwork);
        /// <summary>
        /// ������ȼƻ�
        /// </summary>
        /// <param name="currUser">��ǰ�û�</param>
        /// <param name="oldYear">���Ƶ����</param>
        /// <param name="newYear">���Ƶ������</param>
        /// <returns></returns>
        bool CopyHistoryData(Operator currUser, string oldYear, string newYear);

            /// <summary>
        /// ֻ�޸ļƻ��·�ֱ��ͬ����EHS�뷢��������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        void SynchData(string planid, string planfjid);
        #endregion
    }
}
