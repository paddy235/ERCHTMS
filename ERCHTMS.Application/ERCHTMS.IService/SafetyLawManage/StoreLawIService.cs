using ERCHTMS.Entity.SafetyLawManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.SafetyLawManage
{
    /// <summary>
    /// �� �����ղط��ɷ���
    /// </summary>
    public interface StoreLawIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ��ȫ�������ɷ����ҵ��ղ��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetPageDataTable(Pagination pagination, string queryJson);

        /// <summary>
        /// ��ȡ��ȫ�����ƶ��ҵ��ղ��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetPageJsonInstitution(Pagination pagination, string queryJson);

        /// <summary>
        /// ��ȡ��ȫ��������ҵ��ղ��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetPageJsonStandards(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<StoreLawEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        StoreLawEntity GetEntity(string keyValue);

        /// <summary>
        /// ���ݷ���idȷ���Ƿ����ղ�
        /// </summary>
        /// <returns></returns>
        int GetStoreBylawId(string lawid);
       
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
        void SaveForm(string keyValue, StoreLawEntity entity);
        #endregion
    }
}
