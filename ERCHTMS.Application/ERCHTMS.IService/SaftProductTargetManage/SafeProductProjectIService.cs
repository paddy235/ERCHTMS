using ERCHTMS.Entity.SaftProductTargetManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.SaftProductTargetManage
{
    /// <summary>
    /// �� ������ȫ����Ŀ����Ŀ
    /// </summary>
    public interface SafeProductProjectIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SafeProductProjectEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SafeProductProjectEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡ��ȫĿ����Ŀ�б�
        /// </summary>
        /// <param name="ProductId">��ȫĿ��id</param>
        /// <returns></returns>
        IEnumerable<SafeProductProjectEntity> GetListByProductId(string productId);

        /// <summary>
        /// �б��ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        IEnumerable<SafeProductProjectEntity> GetPageList(Pagination pagination, string queryJson);
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValue);

        /// <summary>
        /// ���ݰ�ȫ����Ŀ��idɾ����ȫĿ����Ŀ
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        int Remove(string productId);
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string keyValue, SafeProductProjectEntity entity);

        #endregion
    }
}
