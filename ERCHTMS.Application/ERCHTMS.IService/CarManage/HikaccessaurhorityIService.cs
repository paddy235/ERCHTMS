using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// �� �����Ž���Ȩ�ޱ�
    /// </summary>
    public interface HikaccessaurhorityIService
    {
        #region ��ȡ����

        /// <summary>
        /// Ȩ���б�
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
        IEnumerable<HikaccessaurhorityEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        HikaccessaurhorityEntity GetEntity(string keyValue);
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValue, string pitem, string url);

        /// <summary>
        /// �����û�ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveUserForm(string keyValue, string pitem, string url);

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string StartTime, string EndTime, List<Access> DeptList, List<Access> AccessList, int Type,
            string pitem, string url);

        #endregion
    }
}
