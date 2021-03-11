using ERCHTMS.Entity.OccupationalHealthManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OccupationalHealthManage
{
    /// <summary>
    /// �� ����ְҵ����Ա��
    /// </summary>
    public interface OccupatioalstaffIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<OccupatioalstaffEntity> GetList(string queryJson);

        /// <summary>
        /// ����������ѯ��������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetTable(string queryJson,string where);

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        OccupatioalstaffEntity GetEntity(string keyValue);

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
        void SaveForm(string keyValue, OccupatioalstaffEntity entity);

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="IsNew">�Ƿ�����</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(bool IsNew, OccupatioalstaffEntity entity);
        #endregion
    }
}
