using ERCHTMS.Entity.HazardsourceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.HazardsourceManage
{
    /// <summary>
    /// �� ����Σ��Դ�嵥
    /// </summary>
    public interface IHisrelationhd_qdService
    {
        #region ��ȡ����

        IEnumerable<Hisrelationhd_qdEntity> GetListForRecord(string queryJson);

        /// <summary>
        /// ��������ͳ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetReportForDistrictName(string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<Hisrelationhd_qdEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        Hisrelationhd_qdEntity GetEntity(string keyValue);


        string StaQueryList(string queryJson);
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
        void SaveForm(string keyValue, Hisrelationhd_qdEntity entity);
        #endregion
    }
}
