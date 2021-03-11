using ERCHTMS.Entity.OccupationalHealthManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OccupationalHealthManage
{
    /// <summary>
    /// �� ����ְҵ��Σ�����ؼ��
    /// </summary>
    public interface HazarddetectionIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<HazarddetectionEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        HazarddetectionEntity GetEntity(string keyValue);
        /// <summary>
        /// �洢���̷�ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetPageListByProc(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡȫ���б�����
        /// </summary>
        /// <param name="riskid">Σ������</param>
        /// <param name="areaid">����</param>
        /// <param name="starttime">ʱ�䷶Χ</param>
        /// <param name="endtime">ʱ�䷶Χ</param>
        /// <param name="isexcessive">�Ƿ񳬱�</param>
        /// <param name="detectionuserid">�����id</param>
        /// <returns></returns>
        DataTable GetDataTable(string queryJson, string where);

        /// <summary>
        /// ��ȡ����ָ�꼰��׼
        /// </summary>
        /// <param name="RiskId">ְҵ��id</param>
        /// <returns></returns>
        string GetStandard(string RiskId, string where);

        /// <summary>
        /// ��ȡΣ�����ؼ��ͳ������
        /// </summary>
        /// <param name="year">��һ������</param>
        /// <param name="risk">ְҵ������</param>
        /// <param name="type">true��ѯȫ�� false��ѯ��������</param>
        /// <returns></returns>
        DataTable GetStatisticsHazardTable(int year, string risk, bool type, string where);
        #endregion

        #region �ύ����
        /// <summary>
        /// ����id��������ɾ��
        /// </summary>
        /// <param name="Ids"></param>
        void Remove(string Ids);
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
        void SaveForm(string keyValue, HazarddetectionEntity entity);
        #endregion

        #region �ֻ���
        /// <summary>
        /// ����Σ�����ؼ������
        /// </summary>
        /// <param name="assess">ʵ��</param>
        /// <param name="user">��ǰ�û�</param>
        /// <returns></returns>
        int SaveHazard(HazarddetectionEntity hazard, ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡȫ���б�����
        /// </summary>
        /// <param name="riskid">Σ������</param>
        /// <param name="areaid">����</param>
        /// <param name="starttime">ʱ�䷶Χ</param>
        /// <param name="endtime">ʱ�䷶Χ</param>
        /// <param name="isexcessive">�Ƿ񳬱�</param>
        /// <param name="detectionuserid">�����id</param>
        /// <returns></returns>
        DataTable GetDataTable(string riskid, string areaid, string starttime, string endtime, string isexcessive, string detectionuserid, string where);
        #endregion
    }
}
