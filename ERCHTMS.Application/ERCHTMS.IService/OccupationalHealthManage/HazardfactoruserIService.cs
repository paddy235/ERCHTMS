using ERCHTMS.Entity.OccupationalHealthManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OccupationalHealthManage
{
    /// <summary>
    /// �� ����Σ��������Ա��
    /// </summary>
    public interface HazardfactoruserIService
    {
        #region ��ȡ����

        /// <summary>
        /// ��ѯְҵ���Ӵ��û���
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<HazardfactoruserEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        HazardfactoruserEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡ�û��ĽӴ�Σ������
        /// </summary>
        /// <returns></returns>
        string GetUserHazardfactor(string useraccount);

        /// <summary>
        /// ��ѯְҵ���Ӵ��û���
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetPageList(string sqlwhere, string queryJson);
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
        void SaveForm(string keyValue, HazardfactoruserEntity entity);
        #endregion
    }
}
