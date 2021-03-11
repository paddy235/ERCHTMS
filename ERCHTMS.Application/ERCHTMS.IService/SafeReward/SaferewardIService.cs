using ERCHTMS.Entity.SafeReward;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.OutsourcingProject;

namespace ERCHTMS.IService.SafeReward
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    public interface SaferewardIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SaferewardEntity> GetList(string queryJson);


        object GetStandardJson();
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SaferewardEntity GetEntity(string keyValue);

        DataTable GetPageList(Pagination pagination, string queryJson);
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
        void SaveForm(string keyValue, SaferewardEntity entity);
        #endregion

        void CommitApply(string keyValue, AptitudeinvestigateauditEntity entity, string leaderShipId);


        DataTable GetRewardStatisticsList(string year);

        DataTable GetRewardStatisticsTimeList(string year);

        string GetRewardStatisticsCount(string year);

        string GetRewardStatisticsTime(string year);

        Flow GetFlow(string keyValue);
        List<object> GetLeaderList();

        string GetRewardCode();

        string GetRewardNum();

        string GetDeptPId(string deptId);

        List<object> GetSpecialtyPrincipal(string applyDeptId);

        /// <summary>
        /// ��ȡ�����Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetAptitudeInfo(string keyValue);

        string GetRewardStatisticsExcel(string year = "");

        string GetRewardStatisticsTimeExcel(string year = "");
    }
}
