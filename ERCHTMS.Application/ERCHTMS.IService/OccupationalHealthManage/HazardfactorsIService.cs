using ERCHTMS.Entity.OccupationalHealthManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OccupationalHealthManage
{
    /// <summary>
    /// �� ����Σ�������嵥
    /// </summary>
    public interface HazardfactorsIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<HazardfactorsEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="AreaId">����id</param>
        /// <param name="where">������ѯ����</param>
        /// <returns></returns>
        DataTable GetList(string AreaId, string where);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        HazardfactorsEntity GetEntity(string keyValue);

        /// <summary>
        /// ��֤���������Ƿ��ظ�
        /// </summary>
        /// <param name="AreaValue">��������</param>
        /// <returns></returns>
        bool ExistDeptJugement(string AreaValue, string orgCode, string RiskName);

        /// <summary>
        /// ��֤����id�Ƿ��ظ�//���ֲ�ͬ��˾�û�
        /// </summary>
        /// <param name="Areaid">��������</param>
        /// <returns></returns>
        bool ExistAreaidJugement(string Areaid, string orgCode, string RiskName);

        /// <summary>
        /// ��֤����id��Σ��Դ�Ƿ��ظ�//���ֲ�ͬ��˾�û�
        /// </summary>
        /// <param name="Areaid">��������</param>
        /// <returns></returns>
        bool ExistAreaidJugement(string Areaid, string orgCode, string RiskName, string Hid);

        /// <summary>
        /// ��֤�Ƿ��и�Σ��Դ������з���Code ���û�з��ؿ��ַ���
        /// </summary>
        /// <param name="code">�ֵ��Code</param>
        /// <param name="RiskName">Σ��Դ����</param>
        /// <returns></returns>
        string IsRisk(string code, string RiskName);

        /// <summary>
        /// ����������ѯ��������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetTable(string queryJson, string where);

        /// <summary>
        /// �洢���̷�ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetPageListByProc(Pagination pagination, string queryJson);
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
        void SaveForm(string keyValue, HazardfactorsEntity entity, string UserName, string UserId);
        #endregion
        #region �ֻ���
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<HazardfactorsEntity> PhoneGetList(string queryJson, string orgid);
        #endregion
    }
}
