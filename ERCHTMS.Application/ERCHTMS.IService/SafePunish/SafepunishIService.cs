using ERCHTMS.Entity.SafePunish;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.OutsourcingProject;

namespace ERCHTMS.IService.SafePunish
{
    /// <summary>
    /// �� ������ȫ�ͷ�
    /// </summary>
    public interface SafepunishIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SafepunishEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SafepunishEntity GetEntity(string keyValue);

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
        void SaveForm(string keyValue, SafepunishEntity entity, SafekpidataEntity kpiEntity);
        #endregion

        void CommitApply(string keyValue, AptitudeinvestigateauditEntity entity);

        string GetPunishStatisticsCount(string year, string statMode);

        string GetPunishStatisticsList(string year, string statMode);
        Flow GetFlow(string keyValue);
        string GetPunishCode();

        string GetPunishNum();

        /// <summary>
        /// ��ȡ�����Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetAptitudeInfo(string keyValue);
    }
}
