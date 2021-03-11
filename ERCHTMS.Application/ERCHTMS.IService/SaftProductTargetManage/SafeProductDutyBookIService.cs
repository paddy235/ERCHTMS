using ERCHTMS.Entity.SaftProductTargetManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.SaftProductTargetManage
{
    /// <summary>
    /// �� ������ȫ����������
    /// </summary>
    public interface SafeProductDutyBookIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SafeProductDutyBookEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SafeProductDutyBookEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡ��ȫĿ���������б�
        /// </summary>
        /// <param name="ProductId">��ȫĿ��id</param>
        /// <returns></returns>
        IEnumerable<SafeProductDutyBookEntity> GetListByProductId(string productId);

        /// <summary>
        /// �б��ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        IEnumerable<SafeProductDutyBookEntity> GetPageList(Pagination pagination, string queryJson);
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
        void SaveForm(string keyValue, SafeProductDutyBookEntity entity);
        #endregion
    }
}
